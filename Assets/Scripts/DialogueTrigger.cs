
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] dialogueLines;  
    public DialougeManager dialogueManager;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.StartDialogue(dialogueLines);
        }
    }
}
