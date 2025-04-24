using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraTimer : MonoBehaviour
{
    public TMP_Text timerText;
    private float timer = 0f;

    void Start()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 1000f) % 1000f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }
}
