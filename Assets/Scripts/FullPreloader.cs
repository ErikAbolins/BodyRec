using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class FullPreloader : MonoBehaviour
{
  public static FullPreloader Instance;
 
  private List<Object> loadedAssets = new List<Object>();
 
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }
 
  public IEnumerator PreloadEverything(System.Action<float> onProgress = null)
  {
    loadedAssets.Clear();
    float totalSteps = 5f;
    float currentStep = 0f;
 
    // Step 1: Load all shaders
    Shader[] shaders = Resources.LoadAll<Shader>("");
    foreach (Shader shader in shaders)
    {
      if (shader != null)
        Shader.WarmupAllShaders(); // Unity warms all shaders globally, this is still safe
    }
    currentStep++;
    onProgress?.Invoke(currentStep / totalSteps);
    yield return null;
 
    // Step 2: Load all materials
    Material[] materials = Resources.LoadAll<Material>("");
    loadedAssets.AddRange(materials);
    currentStep++;
    onProgress?.Invoke(currentStep / totalSteps);
    yield return null;
 
    // Step 3: Load all textures
    Texture[] textures = Resources.LoadAll<Texture>("");
    loadedAssets.AddRange(textures);
    currentStep++;
    onProgress?.Invoke(currentStep / totalSteps);
    yield return null;
 
    // Step 4: Load all meshes
    Mesh[] meshes = Resources.LoadAll<Mesh>("");
    loadedAssets.AddRange(meshes);
    currentStep++;
    onProgress?.Invoke(currentStep / totalSteps);
    yield return null;
 
    // Step 5: Load all prefabs
    GameObject[] prefabs = Resources.LoadAll<GameObject>("");
    foreach (GameObject go in prefabs)
    {
      GameObject instance = Instantiate(go);
      instance.SetActive(false);
      DontDestroyOnLoad(instance);
      loadedAssets.Add(instance);
      yield return null;
    }
 
    currentStep++;
    onProgress?.Invoke(currentStep / totalSteps);
  }
}