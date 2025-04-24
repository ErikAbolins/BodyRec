using UnityEngine;

public class BloodSplatter : MonoBehaviour
{
    public GameObject bloodSplatterPrefab;  
    public float splatterHeightOffset = 0.1f;  
    public float splatterLifetime = 1f;

    
    public void SpawnBloodSplatter(Vector3 hitPoint, Vector3 hitNormal)
    {
        
        GameObject bloodEffect = Instantiate(bloodSplatterPrefab, hitPoint + hitNormal * splatterHeightOffset, Quaternion.LookRotation(hitNormal));
        Destroy(bloodEffect, splatterLifetime);
       
    }
}
