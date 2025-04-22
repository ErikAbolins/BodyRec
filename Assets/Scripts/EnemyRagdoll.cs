using UnityEngine;
using Pathfinding;
 
public class EnemyRagdoll : MonoBehaviour
{
  public Collider rootCollider; // Assign this in inspector
 
  private Animator animator;
  private Rigidbody[] ragdollBodies;
  private Collider[] ragdollColliders;
  private AIPath aiPath;
  private bool isDead = false;
 
  void Start()
  {
    animator = GetComponentInChildren<Animator>();
    ragdollBodies = GetComponentsInChildren<Rigidbody>();
    ragdollColliders = GetComponentsInChildren<Collider>();
 
    aiPath = GetComponent<AIPath>();
 
    DisableRagdoll();
 
    if (rootCollider != null)
    {
      rootCollider.enabled = true; // Enable root collider at start so raycasts work
    }
    else
    {
Debug.LogWarning("Root Collider not assigned on " + gameObject.name);
    }
  }
 
  public void EnableRagdoll(Vector3 hitPoint, Vector3 hitForce)
  {
    if (isDead) return;
 
    isDead = true;
 
    if (aiPath != null)
    {
      aiPath.canMove = false;
      aiPath.enabled = false;
    }
 
    if (animator != null)
    {
      animator.enabled = false;
    }
 
    foreach (var rb in ragdollBodies)
    {
      rb.isKinematic = false;
    }
 
    foreach (var col in ragdollColliders)
    {
      if (col != rootCollider) // Don't accidentally re-enable root collider
      {
        col.enabled = true;
      }
    }
 
    if (rootCollider != null)
    {
      rootCollider.enabled = false; // Only disable AFTER enabling ragdoll colliders
    }
 
    Rigidbody closestBody = GetClosestRigidbody(hitPoint);
    if (closestBody != null)
    {
      closestBody.AddForce(hitForce * 50f, ForceMode.Impulse);
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
}