using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerScreen : MonoBehaviour
{
    public Camera scannerCamera;
    private bool isScanning = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (!isScanning)
            {
                isScanning = true;
                scannerCamera.enabled = true;
            }
        }
        else
        {
            if (isScanning)
            {
                isScanning = false;
                scannerCamera.enabled = false;
            }
        }
    }
}
