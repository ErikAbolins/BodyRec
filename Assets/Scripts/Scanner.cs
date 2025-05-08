using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
  public Camera playerCamera;
  public Transform scannerTransform;

  [Header("Aiming")]
  public Transform aimTransform;
  public float aimSpeed = 10f;

  [Header("Battery")]
  public float maxBattery = 100f;
  public float batteryDrainRate = 10f; // per second when scanning
  public Image batteryBar; // assign in inspector
  private float currentBattery;

  private bool isAiming = false;
  private float originalFov;
  private Vector3 originalPosition;

  void Start()
  {
    originalPosition = scannerTransform.localPosition;
    originalFov = playerCamera.fieldOfView;
    currentBattery = maxBattery;
  }

  void Update()
  {
    HandleAiming();
    UpdateBatteryUI();
  }

  void HandleAiming()
  {
    bool wantsToAim = Input.GetKey(KeyCode.F);

    // Can't aim if battery is empty
    if (wantsToAim && currentBattery > 0f)
    {
      isAiming = true;
      currentBattery -= batteryDrainRate * Time.deltaTime;
      currentBattery = Mathf.Max(currentBattery, 0f); // Clamp
    }
    else
    {
      isAiming = false;
    }

    Vector3 targetPosition = isAiming ? aimTransform.localPosition : originalPosition;
    scannerTransform.localPosition = Vector3.Lerp(scannerTransform.localPosition, targetPosition, Time.deltaTime * aimSpeed);

    float targetFov = isAiming ? 5f : originalFov;
    playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * aimSpeed);
  }

  void UpdateBatteryUI()
  {
    if (batteryBar != null)
    {
      batteryBar.fillAmount = currentBattery / maxBattery;
    }
  }

  public bool ScannerIsActive()
  {
    return isAiming;
  }

  public bool HasBattery()
  {
    return currentBattery > 0f;
  }
}
