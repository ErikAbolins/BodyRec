using UnityEngine;
using Pathfinding;

public class EnemyRagdoll : MonoBehaviour
{
    public Collider rootCollider;
    public int health = 20; // Increased health to make the enemies tankier
    public int headshotMultiplier = 3;

    private Animator animator;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private AIPath aiPath;
    private bool isDead = false;
    public AudioSource sfxSource;
    public AudioSource footstepSource;
    public AudioClip deathClip;


    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        aiPath = GetComponent<AIPath>();

        DisableRagdoll();

        if (rootCollider != null)
        {
            rootCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("Root Collider not assigned on " + gameObject.name);
        }
    }

    public void TakeDamage(int damage, bool isHeadshot)
    {
        if (isDead) return;

        if (isHeadshot)
        {
            damage *= headshotMultiplier; // Apply headshot damage multiplier
        }

        health -= damage;
        Debug.Log("Enemy Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy is dead");
        isDead = true;

        // Stop footstep sounds
        if (footstepSource != null)
        {
            footstepSource.Stop();
        }

        // Stop the idle sound directly (in case it's still playing)
        if (sfxSource != null && sfxSource.isPlaying)
        {
            sfxSource.Stop();  // Stop any lingering idle sounds
        }

        // Play death sound
        if (sfxSource != null && deathClip != null)
        {
            sfxSource.PlayOneShot(deathClip);
        }

        // Enable ragdoll physics
        EnableRagdoll(Vector3.zero, Vector3.zero);
    }


    public void EnableRagdoll(Vector3 hitPoint, Vector3 hitForce)
    {
        Debug.Log(gameObject.name + " ragdoll enabled!");  // Debug log to check if it's being triggered

        isDead = true;

        if (aiPath != null)
        {
            aiPath.canMove = false;  // Stop AI movement
            aiPath.enabled = false;  // Disable AI pathfinding
        }

        if (animator != null)
        {
            animator.enabled = false;  // Disable animation
        }

        // Enable physics for ragdoll parts
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = false;  // Enable physics on all rigidbodies
        }

        // Enable colliders for ragdoll parts
        foreach (var col in ragdollColliders)
        {
            if (col != rootCollider) // Don't enable the root collider
            {
                col.enabled = true;
            }
        }

        if (rootCollider != null)
        {
            rootCollider.enabled = false;  // Disable root collider
        }

        // Apply force to the ragdoll to simulate the impact
        Rigidbody closestBody = GetClosestRigidbody(hitPoint);
        if (closestBody != null)
        {
            closestBody.AddForce(hitForce * 50f, ForceMode.Impulse);  // Adjust force if necessary
        }
    }


    public void DisableRagdoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }

        foreach (var col in ragdollColliders)
        {
            if (col != rootCollider)
            {
                col.enabled = false;
            }
        }

        if (animator != null)
        {
            animator.enabled = true;
        }

        if (rootCollider != null)
        {
            rootCollider.enabled = true;
        }
    }

    private Rigidbody GetClosestRigidbody(Vector3 hitPoint)
    {
        Rigidbody closest = null;
        float minDist = float.MaxValue;

        foreach (var rb in ragdollBodies)
        {
            float dist = Vector3.Distance(hitPoint, rb.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = rb;
            }
        }

        return closest;
    }

    // Add a method to check if the enemy is dead
    public bool IsDead()
    {
        return isDead;
    }


}