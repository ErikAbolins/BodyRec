using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Camera playerCamera;
    public Transform gunTransform;

    [Header("Shooting Settings")]
    public float fireRate = 0.2f;
    public float recoilAmount = 2f;
    public float recoilRecovery = 5f;
    public LayerMask hitMask;

    [Header("Aiming Settings")]
    public Vector3 aimPosition;
    public float aimSpeed = 10f;

    [Header("Weapon Bobbing")]
    public float bobSpeed = 10f;
    public float bobAmount = 0.1f;
    public float bobHeight = 0.1f;

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
        ApplyWeaponBobbing();
        ApplyRecoil();
    }


    private void HandleAiming()
    {
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        Vector3 targetPosition = isAiming ? aimPosition : originalPosition;
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, targetPosition, Time.deltaTime * aimSpeed);

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, isAiming ? 50f : originalFov, Time.deltaTime * aimSpeed);
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
            Debug.Log("hit: " + hit.collider.name);
        }

        recoilOffset -= new Vector3(0f, recoilAmount, recoilAmount);
    }



    private void ApplyWeaponBobbing()
    {
        if (isAiming || Time.timeScale == 0)
            return;


        float moveX = Mathf.Sin(Time.time * bobSpeed * bobAmount);
        float moveY = Mathf.Cos(Time.time * bobSpeed * bobAmount);


        gunTransform.localPosition = new Vector3(originalPosition.x + moveX, originalPosition.y + moveY + bobHeight, originalPosition.z);
    }



    private void ApplyRecoil()
    {
        recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero, Time.deltaTime * recoilRecovery);

        gunTransform.localPosition += recoilOffset;
    }
}
