using System.IO;
using UnityEngine;

public class CoconutEnforcer : MonoBehaviour
{
    void Start()
    {
        string coconutPath = Path.Combine(Application.streamingAssetsPath, "coconut.jpg");

        if (!File.Exists(coconutPath))
        {
            Application.Quit();
            //DO NOT KILL THE COCONUT

        }
    }
}
