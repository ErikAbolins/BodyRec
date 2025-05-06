using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodexPickup : MonoBehaviour
{
    public CodexEntry codexEntry;
    public GameObject player;
    private bool isInRange = false;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

     void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            CodexUI.Instance.ShowEntry(codexEntry);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isInRange = true;
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isInRange = false;
        }

    }
}
