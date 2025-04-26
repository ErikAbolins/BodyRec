using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
  public Transform[] patrolPoints;
  public float viewDistance = 10f;
  public float viewAngle = 90f;
  public Transform player;
  public LayerMask visionMask;

  private int currentPatrolIndex = 0;
  private AIPath aiPath;
  private Animator animator;
  private enum State { Patrolling, Chasing }
  private State currentState = State.Patrolling;

  void Start()
  {
    aiPath = GetComponent<AIPath>();
    animator = GetComponent<Animator>();
    GoToNextPatrolPoint();
  }

  void Update()
  {
    switch (currentState)
    {
      case State.Patrolling:
        Patrol();
        if (CanSeePlayer())
        {
          currentState = State.Chasing;
          aiPath.destination = player.position;
          animator.SetBool("IsRunning", true);
        }
        break;

      case State.Chasing:
        aiPath.destination = player.position;
        if (!CanSeePlayer())
        {
          currentState = State.Patrolling;
          GoToNextPatrolPoint();
          animator.SetBool("IsRunning", false);
        }
        break;
    }

    float speed = aiPath.velocity.magnitude;
    animator.SetFloat("Speed", speed);

    //Debug.Log("AI Speed: " + speed);
    //Debug.Log("Animator Speed: " + speed);

    if (speed > 1f)
    {
      animator.SetBool("IsRunning", true);
    }
    else if (speed > 0.1f)
    {
      animator.SetBool("IsRunning", false);
    }
    else
    {
      animator.SetFloat("Speed", 0);
    }
  }

  private void Patrol()
  {
    if (!aiPath.pathPending && aiPath.reachedDestination)
    {
      GoToNextPatrolPoint();
    }
  }

  private void GoToNextPatrolPoint()
  {
    if (patrolPoints.Length == 0) return;
    aiPath.destination = patrolPoints[currentPatrolIndex].position;
    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
  }

  private bool CanSeePlayer()
  {
    Vector3 dirToPlayer = (player.position - transform.position).normalized;
    float angle = Vector3.Angle(transform.forward, dirToPlayer);

    if (angle < viewAngle / 2f)
    {
      if (Vector3.Distance(transform.position, player.position) < viewDistance)
      {
        if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, viewDistance, visionMask))
        {
          return hit.transform == player;
        }
      }
    }
    return false;
  }
}
