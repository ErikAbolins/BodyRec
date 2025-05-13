using UnityEngine;
using System.Text;

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
    style.alignment = TextAnchor.MiddleLeft;
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

    // System data
    float fps = 1.0f / deltaTime;
    long memoryMB = System.GC.GetTotalMemory(false) / (1024 * 1024);
    string cpuUsage = "N/A"; // CPU usage can be fetched via native plugins or third-party libs
    string gpuUsage = "N/A"; // Same as above, you'd need something like NVAPI

    string overlayText = string.Format(
      "FPS: {0:0.} | Frame Time: {1:0.0}ms | Memory (Mono): {2} MB | Position: {3}\n" +
      "CPU Usage: {4} | GPU Usage: {5}\n" +
      "Scene: {6} | System: {7}",
      fps,
      deltaTime * 1000.0f,
      memoryMB,
      transform.position.ToString("F1"),
      cpuUsage,
      gpuUsage,
      UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
      GetSystemSpecs()
    );

    // Full-width rect for the bar at the bottom of the screen
    float barHeight = 100f; // Height of the bar (adjustable)
    rect = new Rect(0, Screen.height - barHeight, Screen.width, barHeight);

    // Draw the background for the bar
    GUI.backgroundColor = new Color(0f, 0f, 0f, 0.7f); // Semi-transparent black
    GUI.Box(rect, "", style); // The bar background

    // Adjust the padding for text inside the bar
    Rect textRect = new Rect(10, Screen.height - barHeight + 10, Screen.width - 20, barHeight - 20); // Padding inside the bar
    GUI.Label(textRect, overlayText, style);
  }

  // Function to get basic system specs
  string GetSystemSpecs()
  {
    StringBuilder specs = new StringBuilder();
    specs.AppendLine($"OS: {SystemInfo.operatingSystem}");
    specs.AppendLine($"CPU: {SystemInfo.processorType} | Cores: {SystemInfo.processorCount}");
    specs.AppendLine($"GPU: {SystemInfo.graphicsDeviceName} | VRAM: {SystemInfo.graphicsMemorySize} MB");
    specs.AppendLine($"RAM: {SystemInfo.systemMemorySize} MB");

    return specs.ToString();
  }
}
