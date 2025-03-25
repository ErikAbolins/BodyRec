using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public Camera playerCamera;
    public Transform scannerTransform;
    
    
    [Header("Aiming")]
    public Transform aimTransform;
    public float aimSpeed = 10f;

    private bool isAiming = false;
    private float originalFov;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = scannerTransform.localPosition;
        originalFov = playerCamera.fieldOfView;
    }


    void Update()
    {
        HandleAiming();
    }



    void HandleAiming()
    {
        isAiming = Input.GetKey(KeyCode.F);

        Vector3 targetPosition = isAiming ? aimTransform.localPosition : originalPosition;
        scannerTransform.localPosition = Vector3.Lerp(scannerTransform.localPosition, targetPosition, Time.deltaTime * aimSpeed);

        float targetFov = isAiming ? 5f : originalFov;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFov, Time.deltaTime * aimSpeed);
    }
}
