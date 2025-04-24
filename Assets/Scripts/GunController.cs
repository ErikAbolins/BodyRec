using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform gunTransform;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;
    public float recoilAmount = 0.1f;
    public float recoilRecovery = 5f;
    public LayerMask hitMask;
    public float bulletHoleLifetime = 10f;
    public GameObject bulletHolePrefab;

    [Header("Aiming Settings")]
    public Transform aimTransform;
    public float aimSpeed = 10f;

    [Header("Sound Settings")]
    public AudioClip shootSound;  // Drag your shooting sound effect here in the Inspector
    private AudioSource audioSource;  // Reference to the AudioSource component

    [Header("Blood Splatter Settings")]
    public BloodSplatter bloodSplatterScript;  // Reference to the BloodSplatter script

    private float nextFireTime = 0f;
    private Vector3 originalPosition;
    private Vector3 recoilOffset;
    private float originalFov;
    private bool isAiming = false;

    void Start()
    {
        originalPosition = gunTransform.localPosition;
        originalFov = playerCamera.fieldOfView;
        
        // Get the AudioSource component from the gun
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();
        ApplyRecoil();
    }

    private void HandleAiming()
    {
        isAiming = Input.GetMouseButton(1);

        Vector3 targetPosition = isAiming ? aimTransform.localPosition : originalPosition;
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, targetPosition + recoilOffset, Time.deltaTime * aimSpeed);

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

        // Play shooting sound
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // Check for enemy hit
            EnemyRagdoll enemy = hit.collider.GetComponentInParent<EnemyRagdoll>();
            if (enemy != null)
            {
                Vector3 hitDirection = ray.direction;
                enemy.EnableRagdoll(hit.point, hitDirection);

                // Trigger blood splatter
                if (bloodSplatterScript != null)
                {
                    bloodSplatterScript.SpawnBloodSplatter(hit.point, hit.normal);
                }
            }
            else if (bulletHolePrefab != null)
            {
                Quaternion bulletRotation = Quaternion.LookRotation(-hit.normal);
                Vector3 bulletPosition = hit.point + hit.normal * 0.01f;

                GameObject hole = Instantiate(bulletHolePrefab, bulletPosition, bulletRotation);
                hole.transform.SetParent(hit.collider.transform);
                Destroy(hole, bulletHoleLifetime);
            }
        }

        // Recoil
        float recoilX = Random.Range(-0.05f, 0.05f) * recoilAmount;
        float recoilY = Random.Range(0.1f, 0.2f) * recoilAmount;
        float recoilZ = 0.1f * recoilAmount;

        recoilOffset += new Vector3(recoilX, recoilY, -recoilZ);
    }

    private void ApplyRecoil()
    {
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecovery);
    }
}
