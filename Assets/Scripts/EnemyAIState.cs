using UnityEngine;
using Pathfinding;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float viewDistance = 20f;
    public float viewAngle = 90f;
    public Transform player;
    public LayerMask visionMask;

    public AudioClip[] idleSounds;
    public AudioClip footstepClip;
    public AudioClip aggroClip;

    private AudioSource sfxSource;
    private AudioSource footstepSource;

    private int currentPatrolIndex = 0;
    private AIPath aiPath;
    private Animator animator;
    private enum State { Patrolling, Chasing }
    private State currentState = State.Patrolling;

    private bool hasAggroed = false;
    private Coroutine idleSoundRoutine;
    private EnemyRagdoll enemyRagdoll;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();

        // Set up audio sources for localized sounds
        sfxSource = gameObject.AddComponent<AudioSource>();
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepClip;
        footstepSource.loop = true;

        // Set audio sources to 3D sound
        sfxSource.spatialBlend = 1.0f; // Fully 3D
        footstepSource.spatialBlend = 1.0f; // Fully 3D

        // Set max and min distances for footstep sounds
        footstepSource.maxDistance = 5f; // Make sure this is appropriate for your scene
        footstepSource.minDistance = 2f;  // Make sure the sound is audible up close

        // Set max and min distances for idle sounds too (if relevant)
        sfxSource.maxDistance = 5f; 
        sfxSource.minDistance = 3f; // Adjust this as needed

        enemyRagdoll = GetComponent<EnemyRagdoll>();

        GoToNextPatrolPoint();

        idleSoundRoutine = StartCoroutine(PlayIdleSounds());
    }

    void Update()
    {
        // Ensure the enemy is dead before continuing with AI logic
        if (enemyRagdoll != null && enemyRagdoll.IsDead())
        {
            footstepSource.Stop();

            // Stop idle sound if playing
            if (idleSoundRoutine != null)
            {
                StopCoroutine(idleSoundRoutine);
                idleSoundRoutine = null;
            }

            return;
        }

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (CanSeePlayer())
                {
                    currentState = State.Chasing;
                    aiPath.destination = player.position;
                    animator.SetBool("IsRunning", true);

                    if (!hasAggroed)
                    {
                        hasAggroed = true;
                        sfxSource.PlayOneShot(aggroClip);
                    }

                    if (idleSoundRoutine != null)
                    {
                        StopCoroutine(idleSoundRoutine);
                        idleSoundRoutine = null;
                    }
                }
                break;

            case State.Chasing:
                aiPath.destination = player.position;
                if (!CanSeePlayer())
                {
                    currentState = State.Patrolling;
                    GoToNextPatrolPoint();
                    animator.SetBool("IsRunning", false);
                    hasAggroed = false;

                    idleSoundRoutine = StartCoroutine(PlayIdleSounds());
                }
                break;
        }

        float speed = aiPath.velocity.magnitude;

        if (speed > 0.1f)
        {
            if (!footstepSource.isPlaying)
            {
                footstepSource.Play();
            }
        }
        else
        {
            footstepSource.Stop();
        }

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", speed > 1f);
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

    private IEnumerator PlayIdleSounds()
    {
        while (true)
        {
            if (idleSounds.Length > 0)
            {
                AudioClip clip = idleSounds[Random.Range(0, idleSounds.Length)];
                sfxSource.PlayOneShot(clip);
            }

            float waitTime = Random.Range(3f, 7f); // Adjust as needed
            yield return new WaitForSeconds(waitTime);
        }
    }
}
