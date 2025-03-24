using UnityEngine;

    public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform gunTransform;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;
    public float recoilAmount = 0.1f; // 🔹 Reduced recoil amount for smaller kicks
    public float recoilRecovery = 5f; // 🔹 Slower recovery for more natural return
    public LayerMask hitMask;

    [Header("Aiming Settings")]
    public Transform aimTransform;
    public float aimSpeed = 10f;

    [Header("Weapon Bobbing")]
    public float bobSpeed = 8f;
    public float bobAmount = 0.05f;
    public float bobHeight = 0.05f;

    private float nextFireTime = 0f;
    private Vector3 originalPosition;
    private Vector3 recoilOffset;
    private float originalFov;
    private bool isAiming = false;

    void Start()
    {
        originalPosition = gunTransform.localPosition;
        originalFov = playerCamera.fieldOfView;
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        ApplyWeaponBobbing();
        ApplyRecoil();
    }

    private void HandleAiming()
    {
        isAiming = Input.GetMouseButton(1);

        // Aiming transition: smoothly move gun to aim position without messing with lean
        Vector3 targetPosition = isAiming ? aimTransform.localPosition : originalPosition;
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, targetPosition + recoilOffset, Time.deltaTime * aimSpeed);

        // Smooth FOV transition based on aiming
        float targetFov = isAiming ? 50f : originalFov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * aimSpeed);
    }

    private void HandleShooting()
    {
        if (Time.time >= nextFireTime && Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;

        // Raycast to check hits
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);
        }

        // 🔹 Reduced recoil with smaller movement values
        float recoilX = Random.Range(-0.05f, 0.05f) * recoilAmount; // Very small horizontal sway
        float recoilY = Random.Range(0.1f, 0.2f) * recoilAmount; // Smaller vertical kick
        float recoilZ = 0.1f * recoilAmount; // Mild backward movement

        recoilOffset += new Vector3(recoilX, recoilY, -recoilZ);
    }

    private void ApplyWeaponBobbing()
    {
        if (isAiming || Time.timeScale == 0)
            return;

        // Smooth bobbing effect
        float moveX = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        float moveY = Mathf.Cos(Time.time * bobSpeed) * bobHeight;

        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, originalPosition + new Vector3(moveX, moveY, 0), Time.deltaTime * 8f);
    }

    private void ApplyRecoil()
    {
        // Smooth recoil recovery, slower return to center
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecovery);
    }
}
