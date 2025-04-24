using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip footstepSound;
    public float stepInterval = 0.5f;
    private AudioSource audioSource;
    private bool isWalking = false;
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isWalking = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        
        if (isWalking)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
    }


    void PlayFootstep()
    {
        if (audioSource != null && footstepSound != null)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }
}
