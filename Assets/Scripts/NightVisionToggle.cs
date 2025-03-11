using UnityEngine;
using UnityEngine.Rendering;

public class NightVisionToggle : MonoBehaviour
{
    [SerializeField] private Camera nightVisionCamera;
    [SerializeField] private Camera regularCamera;
    [SerializeField] private Volume regularVolume;
    [SerializeField] private Volume nightVisionVolume;
    [SerializeField] private float mouseSensitivity = 2f;

    private bool nightVisionActive = false;
    private float rotationX = 0f;

    void Start()
    {
        nightVisionVolume.enabled = false;  
        regularVolume.enabled = true;       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            nightVisionActive = !nightVisionActive;

          
            nightVisionVolume.enabled = nightVisionActive;
            regularVolume.enabled = !nightVisionActive;

         
            nightVisionCamera.enabled = nightVisionActive;
            regularCamera.enabled = !nightVisionActive;
        }

        HandleCameraMovement();
    }

    void HandleCameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        if (nightVisionActive)
        {
            nightVisionCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            nightVisionCamera.transform.parent.Rotate(Vector3.up * mouseX);
        }
        else
        {
            regularCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            regularCamera.transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}
