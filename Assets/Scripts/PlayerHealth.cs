using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("playing flash");
        FindObjectOfType<DamageFeedback>()?.PlayFlash();
        Debug.Log("Player took damage. Current health: "  + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    
    
    private void Die()
    {
        Debug.Log("Player died.");
  
        GetComponent<PlayerController>().enabled = false; 

        PlayerDeathManager deathHandler = GetComponent<PlayerDeathManager>();
        if (deathHandler != null)
        {
            deathHandler.HandleDeath();
        }
    }

}
