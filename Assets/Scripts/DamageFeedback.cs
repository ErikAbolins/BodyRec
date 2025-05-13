using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DamageFeedback : MonoBehaviour
{
   public Image damageFlash;
   public float FlashDuration = 0.3f;
   public Color flashColor = new Color(1f, 0f, 0f, 0.4f);

   private Coroutine flashRoutine;

   public void PlayFlash()
   {
      if (flashRoutine != null)
      {
         StopCoroutine(flashRoutine);
      }

      flashRoutine = StartCoroutine(Flash());
   }


   private IEnumerator Flash()
   {
      damageFlash.color = flashColor;

      float elapsed = 0f;
      while (elapsed < FlashDuration)
      {
         damageFlash.color = Color.Lerp(flashColor, Color.clear, elapsed / FlashDuration);
         elapsed += Time.deltaTime;
         yield return null;
      }
      
      damageFlash.color = Color.clear;
   }
}
