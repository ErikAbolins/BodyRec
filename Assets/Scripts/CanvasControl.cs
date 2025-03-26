using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    public GameObject scannerCanvas;  
    private bool isScannerActive = false;  

    void Update()
    {
       
        if (Input.GetKey(KeyCode.F))
        {
            isScannerActive = !isScannerActive;
            scannerCanvas.SetActive(isScannerActive);  
        }
    }

}
