using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialougeManager : MonoBehaviour
{
    public GameObject chatBox;  
    public TextMeshProUGUI dialogueText;  
    private Queue<string> sentences = new Queue<string>();

    private void Start()
    {
        chatBox.SetActive(false);  
    }

    
    public void StartDialogue(string[] dialogue)
    {
        chatBox.SetActive(true);  
        sentences.Clear();  

        foreach (string sentence in dialogue)
        {
            sentences.Enqueue(sentence);  
        }

        DisplayNextSentence();  
    }

    
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());  
            return;
        }

        string sentence = sentences.Dequeue();  
        dialogueText.text = sentence;  

        
        if (sentences.Count == 0)
        {
            StartCoroutine(EndDialogueAfterDelay());  
        }
    }

    
    private IEnumerator EndDialogueAfterDelay()
    {
        yield return new WaitForSeconds(2f);  
        EndDialogue();  
    }

    
    public void EndDialogue()
    {
        chatBox.SetActive(false);  
    }

}
