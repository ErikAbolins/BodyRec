using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathManager : MonoBehaviour
{
   public GameObject deathScreen;
   public float delayBeforeReload = 3f;
   public AudioClip deathSound;
   public AudioSource audioSource;


  public void HandleDeath()
   {
      if (deathScreen != null)
      {
         deathScreen.SetActive(true);
      }

      if (audioSource != null && deathSound != null)
      {
         audioSource.PlayOneShot(deathSound);
      }

      StartCoroutine(DeathSequence());
   }


   private IEnumerator DeathSequence()
   {
      yield return new WaitForSeconds(delayBeforeReload);
      
      Scene currentScene = SceneManager.GetActiveScene();
      SceneManager.LoadScene(currentScene.buildIndex);
   }
}
