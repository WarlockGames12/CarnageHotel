using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class RandomMovement : MonoBehaviour
{

   [Header("Navmesh Settings: ")] 
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private float enemyRange;
    [SerializeField] private float enemyChaseRange;
    [SerializeField] private float enemyChaseSpeed;
    [SerializeField] private float waitTime;
    [SerializeField] private Transform centrePoint;

    [Header("Enemy Animations: ")]
    [SerializeField] private Animator enemyAnimator;

    [Header("Enemy Walking Sound: ")]
    [SerializeField] private AudioSource enemySteps;
    [SerializeField] private AudioClip[] stepSounds;
    
    private bool isChasing;
    private bool isLookingAround;
    private bool isWaiting;
    private Transform playerTransform;
    private RaycastHit lookForWalls;
    private Vector3 waitPoint;
    private float timeSinceLastSighting;

    // Start is called before the first frame update
    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").transform;

        enemyAnimator.SetBool("Walk", true);
        enemyAnimator.SetBool("Run", false);
        enemyAnimator.SetBool("LookAround", false);
        
        
        if (RandomPoint(centrePoint.position, enemyRange, out var randomPoint))
        {
            enemyAgent.Warp(randomPoint);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        EnemyMoves();
        PlaySounds();
    }

    private void PlaySounds()
    {
        CancelInvoke();
        if (Vector3.Distance(playerTransform.position, transform.position) < enemyChaseRange &&
            Physics.Linecast(transform.position, playerTransform.position, out lookForWalls) &&
            lookForWalls.transform.CompareTag("Player") && !enemySteps.isPlaying)
        {
            StartCoroutine(WalkSounds(0.8f));
        } 
        else if (enemyAgent.velocity.magnitude > 0.1f && !enemySteps.isPlaying)
        {
            StartCoroutine(WalkSounds(0.6f));
        }
    }
    
    private IEnumerator WalkSounds(float delay)
    {
        //Play random step sound
        enemySteps.clip = stepSounds[Random.Range(0, stepSounds.Length)];
        enemySteps.pitch = Random.Range(0.8f, 1.2f);
        enemySteps.Play();
        yield return new WaitForSeconds(delay);
    }

    private void EnemyMoves()
    {
        // Check if player is within chase range and is visible
        if (Vector3.Distance(playerTransform.position, transform.position) < enemyChaseRange && Physics.Linecast(transform.position, playerTransform.position, out lookForWalls) && lookForWalls.transform.CompareTag("Player"))
        {
            isChasing = true;
            timeSinceLastSighting = 0f;
            enemyAgent.speed = enemyChaseSpeed;
            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetBool("Run", true);
            enemyAnimator.SetBool("LookAround", false);
            enemyAgent.SetDestination(playerTransform.position);
        }
        else
        {
            timeSinceLastSighting += Time.deltaTime;

            // Check if enemy is done moving
            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance && !enemyAgent.pathPending && !isWaiting)
            {
                if (isChasing)
                {
                    enemyAnimator.SetBool("Walk", true);
                    enemyAnimator.SetBool("Run", false);
                    enemyAnimator.SetBool("LookAround", false);
                    isChasing = false;

                    if (RandomPoint(centrePoint.position, enemyRange, out waitPoint))
                    {
                        enemyAgent.SetDestination(waitPoint);
                        isWaiting = true;
                    }
                }
                else if (!isLookingAround && !isWaiting)
                {
                    enemyAnimator.SetBool("Walk", false);
                    enemyAnimator.SetBool("Run", false);
                    enemyAnimator.SetBool("LookAround", true);
                    isLookingAround = true;
                }
            }
            else if (!isChasing)
            {
                isLookingAround = false;
                enemyAnimator.SetBool("LookAround", false);
            }

            if (isWaiting && Vector3.Distance(transform.position, waitPoint) < 0.1f)
            {
                isWaiting = false;
            }

            if (timeSinceLastSighting > waitTime)
            {
                enemyAnimator.SetBool("LookAround", false);
                isLookingAround = false;
                isChasing = false;

                if (RandomPoint(centrePoint.position, enemyRange, out waitPoint))
                {
                    enemyAnimator.SetBool("Walk", true);
                    enemyAgent.SetDestination(waitPoint);
                    timeSinceLastSighting = 0f;
                }
            }
        }
    }
    
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        var randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}