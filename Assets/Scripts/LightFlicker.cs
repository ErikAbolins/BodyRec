using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
   public Light light;
   public float maxWait = 1f;
   public float maxFlicker = 0.2f;
   
   float timer;
   float interval;

   void Update()
   {
    timer += Time.deltaTime;
    if (timer > interval)
    {
        ToggleLight();
    }
   }




   void ToggleLight()
   {
    light.enabled = !light.enabled;
    if (light.enabled)
    {
        interval = Random.Range(0, maxWait);
    }
    else
    {
        interval = Random.Range(0, maxFlicker);
    }

    timer = 0;
   }
}
