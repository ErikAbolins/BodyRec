using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLean : MonoBehaviour
{
    public float leanAngle = 15f;

    public float leanOffset = 0.5f;

    public float leanSpeed = 10f;

    private float targetLean = 0f;
    private Vector3 defaultPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float leanInput = 0f;

        if (Input.GetKey(KeyCode.Q)) leanInput = -1f;
        if (Input.GetKey(KeyCode.E)) leanInput = 1f;


        targetLean = Mathf.Lerp(targetLean, leanInput, Time.deltaTime * leanSpeed);

        float newRotation = targetLean * leanAngle;
        float newPositionX = targetLean * leanOffset;


        transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
        transform.localPosition = defaultPosition + new Vector3(newPositionX, 0f, 0f);
    }
}
