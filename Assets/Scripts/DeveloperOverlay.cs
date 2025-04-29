using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class DevDebugOverlay : MonoBehaviour
{
  public bool showOverlay = true;
  public KeyCode toggleKey = KeyCode.F3;

  private float deltaTime = 0.0f;
  private GUIStyle style;
  private Rect rect;

  void Start()
  {
    style = new GUIStyle();
    style.fontSize = 14;
    style.normal.textColor = Color.green;
    style.fontStyle = FontStyle.Normal;
    style.alignment = TextAnchor.LowerLeft;
    style.richText = false;
    style.font = Resources.GetBuiltinResource<Font>("Lucida Console.ttf");
  }

  void Update()
  {
    if (Input.GetKeyDown(toggleKey))
    {
      showOverlay = !showOverlay;
    }

    deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
  }

  void OnGUI()
  {
    if (!showOverlay)
    {
      return;
    }

    float fps = 1.0f / deltaTime;
    long memoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);

    string overlayText = string.Format(
  "FPS: {0:0.} | Frame Time: {1:0.0}ms\n" +
  "Memory (Mono): {2} MB\n" +
  "Position: {3}\n" +
  "Scene: {4}",
  fps,
  deltaTime * 1000.0f,
  memoryMB,
  transform.position.ToString("F1"),
  UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
);


    rect = new Rect(10, Screen.height - 110, Screen.width, 100);
    GUI.Label(rect, overlayText, style);
  }
}
