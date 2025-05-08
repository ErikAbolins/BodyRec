using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerFunction : MonoBehaviour
{
  [Header("Scanner Settings")]
  public float scanRadius = 50f;
  public LayerMask enemyLayer;
  public float enemyDetectionHeight = 2f;

  [Header("Scanner Display")]
  public GameObject ScannerDisplay;
  private Camera scannerCamera;
  private bool isScannerActive = false;

  [Header("Battery Settings")]
  public float maxBattery = 10f;
  public float batteryDrainRate = 1f; // battery per second
  private float currentBattery;

  public UnityEngine.UI.Image batteryUI; // assign in inspector (Canvas on scanner camera)

  void Start()
  {
    scannerCamera = ScannerDisplay.GetComponent<Camera>();
    currentBattery = maxBattery;
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F))
    {
      if (currentBattery > 0f)
      {
        isScannerActive = !isScannerActive;
        ScannerDisplay.SetActive(isScannerActive);
      }
    }

    if (isScannerActive)
    {
      DrainBattery();
      ScanEnemies();
    }

    UpdateBatteryUI();
  }

  void DrainBattery()
  {
    currentBattery -= batteryDrainRate * Time.deltaTime;

    if (currentBattery <= 0f)
    {
      currentBattery = 0f;
      isScannerActive = false;
      ScannerDisplay.SetActive(false);
    }
  }

  void UpdateBatteryUI()
  {
    if (batteryUI != null)
    {
      batteryUI.fillAmount = currentBattery / maxBattery;
    }
  }

  void ScanEnemies()
  {
    Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, scanRadius, enemyLayer);

    foreach (var enemy in detectedEnemies)
    {
      if (enemy.transform.position.y <= enemyDetectionHeight)
      {
        ShowEnemyOnScanner(enemy.transform.position);
      }
    }
  }

  void ShowEnemyOnScanner(Vector3 enemyPosition)
  {
    Vector3 screenPos = scannerCamera.WorldToViewportPoint(enemyPosition);
    // Do something with screenPos later (e.g., blips)
  }
}
