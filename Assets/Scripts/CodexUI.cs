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

    private Coroutine closeRoutine;

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

        if (closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
        }
        closeRoutine = StartCoroutine(AutoClose());
    }

    private IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(8f);
        Close();
    }


    void Close()
    {
        uiPanel.SetActive(false);
    }
}
