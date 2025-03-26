using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        scannerCamera = ScannerDisplay.GetComponent<Camera>();
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            isScannerActive =!isScannerActive;
            ScannerDisplay.SetActive(isScannerActive);
        }

        if (isScannerActive)
        {
            ScanEnemies();
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
    }

}
