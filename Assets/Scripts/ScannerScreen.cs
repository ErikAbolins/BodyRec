using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerScreen : MonoBehaviour
{
  public Camera scannerCamera;
  public Scanner scanner;

  private bool isScanning = false;

  void Update()
  {
    bool shouldBeScanning = scanner != null && scanner.ScannerIsActive() && scanner.HasBattery();

    if (shouldBeScanning && !isScanning)
    {
      isScanning = true;
      scannerCamera.enabled = true;
    }
    else if (!shouldBeScanning && isScanning)
    {
      isScanning = false;
      scannerCamera.enabled = false;
    }
  }
}
