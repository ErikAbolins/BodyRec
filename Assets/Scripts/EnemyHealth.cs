using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    private EnemyRagdoll ragdollController;

    void Start()
    {
        ragdollController = GetComponent<EnemyRagdoll>();
    
    }


    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)

    {
        health -= damage;
        if (health < 0)
        {
            Die(hitPoint, hitDirection);
        }
    }


    void Die(Vector3 hitPoint, Vector3 hitDirection)
    {
        ragdollController.EnableRagdoll();

        Rigidbody hitRigidbody = GetComponentInChildren<Rigidbody>();
        if (hitRigidbody != null)
        {
            hitRigidbody.AddForce(hitDirection * 30f, ForceMode.Impulse);
        }

        if (hitRigidbody != null)
        {
            hitRigidbody.AddForce(hitDirection * 30f, ForceMode.Impulse);


            if (GetComponent<AIPath>() != null)
            {
                GetComponent<AIPath>().enabled = false;
            }
            if (GetComponent<Seeker>() != null)
            {
                GetComponent<Seeker>().enabled = false;
            }
            Destroy(this);
        }
       
    }
}
