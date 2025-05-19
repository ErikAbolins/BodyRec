using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    public GameObject endScreen;
    public GameObject[] uiElementsToDisable;
    public float delayBeforeSceneChange = 5f;
    public string nextSceneName = "MainMenu";

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered)
        {
            return;
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(EndSequence());
        }
    }
        
    private IEnumerator EndSequence()
    {
        foreach (GameObject uiElement in uiElementsToDisable)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(false);
            }
        }

        if (endScreen != null)
        {
            endScreen.SetActive(true);
        }
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(nextSceneName);
    }
}
