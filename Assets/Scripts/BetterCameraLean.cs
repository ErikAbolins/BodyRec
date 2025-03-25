using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCameraLean : MonoBehaviour
{
    [SerializeField] private float leanAngle = 15f;
    [SerializeField] private float leanSpeed = 10f;

    private float currentLean = 0f;
    private float targetLean = 0f;

    void Update()
    {
        HandleLeaning();
    }


    private void HandleLeaning()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            targetLean = leanAngle;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            targetLean = -leanAngle;
        }
        else
        {
            targetLean = 0f;
        }


        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);

        transform.localRotation = Quaternion.Euler(0f, 0f, currentLean);
    }
}
