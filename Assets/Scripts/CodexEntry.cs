using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCodexEntry", menuName = "Codex Entry")]
public class CodexEntry : MonoBehaviour
{
    public string title;
    [TextArea(5, 10)]
    public string content;
    
}
