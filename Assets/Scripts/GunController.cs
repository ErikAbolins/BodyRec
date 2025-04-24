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

  private void ApplyRecoil()
  {
recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecovery);
  }
}