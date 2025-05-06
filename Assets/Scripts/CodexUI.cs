using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodexUI : MonoBehaviour
{
    public static CodexUI Instance;

    public GameObject uiPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI contentText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        uiPanel.SetActive(false);
    }
    


    public void ShowEntry(CodexEntry entry)
    {
        titleText.text = entry.title;
        contentText.text = entry.content;
        uiPanel.SetActive(true);
    }


    void Close()
    {
        uiPanel.SetActive(false);
    }
}
