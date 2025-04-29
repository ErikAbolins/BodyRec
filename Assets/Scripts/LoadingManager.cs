using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
  public Slider progressBar;
  public TMP_Text loadingText;

  [Header("Next Scene Name")]
  public string sceneToLoad = "TestScene"; 

  private void Start()
  {
    StartCoroutine(StartupSequence());
  }

  private IEnumerator StartupSequence()
  {
    loadingText.text = "Booting up...";
    yield return StartCoroutine(FullPreloader.Instance.PreloadEverything(UpdateProgress));

    loadingText.text = "Loading scene...";
    AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
    op.allowSceneActivation = false;

    while (op.progress < 0.9f)
    {
      UpdateProgress(op.progress);
      yield return null;
    }

    
    UpdateProgress(1f);
    loadingText.text = "Ready";
    yield return new WaitForSeconds(0.5f); 

    op.allowSceneActivation = true;
  }

  private void UpdateProgress(float value)
  {
    if (progressBar != null)
      progressBar.value = value;

    if (loadingText != null)
      loadingText.text = $"Loading: {(int)(value * 100)}%";
  }
}
