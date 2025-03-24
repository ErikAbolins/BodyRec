using UnityEngine;

public class CameraLean : MonoBehaviour
{
    public float leanAngle = 15f; // Max angle to lean
    public float leanOffset = 0.5f; // Side movement
    public float leanSpeed = 10f; // Speed of transition

    public Transform gunTransform; // Reference to the gun transform

    private float targetLean = 0f;
    private Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = transform.localPosition; // Store default camera position
    }

    void Update()
    {
        float leanInput = 0f;

        if (Input.GetKey(KeyCode.Q)) leanInput = -1f; // Lean left
        if (Input.GetKey(KeyCode.E)) leanInput = 1f;  // Lean right

        // Calculate target rotation for camera lean
        targetLean = Mathf.Lerp(targetLean, leanInput, Time.deltaTime * leanSpeed);

        float newRotation = targetLean * leanAngle;

        // Apply rotation to the camera
        transform.localRotation = Quaternion.Euler(0f, 0f, newRotation);
        transform.localPosition = defaultPosition + new Vector3(targetLean * leanOffset, 0f, 0f);

        // Adjust the gun's rotation to match the lean
        if (gunTransform != null)
        {
            // To correct the rotation around the correct axis, we apply an additional adjustment
            // Adjusting the pivot by rotating the gun in the opposite direction based on the lean
            gunTransform.localRotation = Quaternion.Euler(0f, 0f, -newRotation); // Reverse the rotation to match the lean
        }
    }
}
