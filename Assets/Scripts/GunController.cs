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

    [Header("Reload Settings")]
    public float reloadDuration = 2f; // Time it takes to reload
    public Vector3 reloadDipPosition = new Vector3(0, -0.2f, 0); // How much the gun dips during reload
    private float reloadTimeRemaining = 0f;
    private bool isReloading = false;

    [Header("Ammo Settings")]
    public int maxAmmoInMag = 7; // Max ammo per mag (M1911 typically has 7)
    private int currentAmmoInMag;

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

        // Initialize ammo count to full mag
        currentAmmoInMag = maxAmmoInMag;
    }

    void Update()
    {
        HandleAiming();
        HandleShooting();

        // If not reloading, handle recoil
        if (!isReloading)
        {
            ApplyRecoil();
        }

        // Handle Reloading
        if (isReloading)
        {
            reloadTimeRemaining -= Time.deltaTime;

            // Move the gun down during reload
            gunTransform.localPosition = Vector3.Lerp(originalPosition, originalPosition + reloadDipPosition, 1 - (reloadTimeRemaining / reloadDuration));

            // If reload is complete, reset and bring the gun back up
            if (reloadTimeRemaining <= 0f)
            {
                isReloading = false;
                gunTransform.localPosition = originalPosition; // Gun comes back up
                ReloadAmmo();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && currentAmmoInMag < maxAmmoInMag) // Trigger reload when pressing 'R'
        {
            StartReload();
        }
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
        if (Time.time >= nextFireTime && Input.GetMouseButton(0) && !isReloading && currentAmmoInMag > 0) // Don't shoot during reload or when out of ammo
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

        // Decrease ammo in mag after shooting
        currentAmmoInMag--;

        // Apply recoil
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

    private void StartReload()
    {
        if (!isReloading && currentAmmoInMag < maxAmmoInMag)
        {
            isReloading = true;
            reloadTimeRemaining = reloadDuration; // Reset the reload timer
        }
    }

    private void ReloadAmmo()
    {
        // Reset to 7 bullets (max mag capacity) after reload
        currentAmmoInMag = maxAmmoInMag;
    }
}
