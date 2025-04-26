using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform gunTransform;

    [Header("Shooting Settings")]
    public float fireRate = 0.5f; // Slower fire rate (increase for less spam)
    public float recoilAmount = 0.1f;
    public float recoilRecovery = 5f;
    public LayerMask hitMask;
    public float bulletHoleLifetime = 10f;
    public GameObject bulletHolePrefab;

    [Header("Aiming Settings")]
    public Transform aimTransform;
    public float aimSpeed = 10f;

    [Header("Sound Settings")]
    public AudioClip shootSound;  
    private AudioSource audioSource;  

    [Header("Blood Splatter Settings")]
    public BloodSplatter bloodSplatterScript;  

    [Header("Muzzle Flash Settings")]
    public GameObject muzzleFlashPrefab;
    public Transform muzzleFlashPoint;

    private float nextFireTime = 0f;
    private Vector3 originalPosition;
    private Vector3 recoilOffset;
    private float originalFov;
    private bool isAiming = false;

    void Start()
    {
        originalPosition = gunTransform.localPosition;
        originalFov = playerCamera.fieldOfView;
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

        if (muzzleFlashPrefab != null && muzzleFlashPoint != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzleFlashPoint.position, muzzleFlashPoint.rotation);
            flash.transform.SetParent(muzzleFlashPoint);
            Destroy(flash, 0.3f);
        }

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        float spread = isAiming ? 0.1f : 0.3f; // Less spread when aiming
        Vector3 shotDirection = playerCamera.transform.forward + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);

        Ray ray = new Ray(playerCamera.transform.position, shotDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, hitMask))
        {
            Debug.Log("Hit: " + hit.collider.name);

            EnemyRagdoll enemy = hit.collider.GetComponentInParent<EnemyRagdoll>();
            if (enemy != null)
            {
                Vector3 hitDirection = ray.direction;

                bool isHeadshot = IsHeadshot(hit.collider);
                int damage = isHeadshot ? 3 : 1;
                enemy.TakeDamage(damage, isHeadshot);

                if (enemy.IsDead())
                {
                    enemy.EnableRagdoll(hit.point, hitDirection);
                }

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

        float recoilX = Random.Range(-0.05f, 0.05f) * recoilAmount;
        float recoilY = Random.Range(0.1f, 0.2f) * recoilAmount;
        float recoilZ = 0.1f * recoilAmount;

        recoilOffset += new Vector3(recoilX, recoilY, -recoilZ);
    }

    private bool IsHeadshot(Collider hitCollider)
    {
        return hitCollider.CompareTag("Head");  
    }

    private void ApplyRecoil()
    {
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecovery);
    }
}
