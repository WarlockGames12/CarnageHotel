using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class RandomMovement : MonoBehaviour
{
 [Header("Navmesh Settings: ")] 
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private float enemyRange;
    [SerializeField] private float enemyChaseRange;
    [SerializeField] private float enemyChaseSpeed;
    [SerializeField] private Transform centrePoint;

    [Header("Enemy Animations: ")]
    [SerializeField] private Animator enemyAnimator;

    private bool isChasing;
    private Transform _playerWhenSeen;
    private bool waitNow;
    private RaycastHit _lookForWalls;
    private Vector3 waitPoint;

    // Start is called before the first frame update
    private void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        _playerWhenSeen = GameObject.FindWithTag("Player").transform;
        enemyAnimator.SetBool("Walk", true);
        enemyAnimator.SetBool("Run", false);
        enemyAnimator.SetBool("LookAround", false);
        enemyAnimator.SetBool("Twerk", false);

        if (RandomPoint(centrePoint.position, enemyRange, out var randomPoint))
        {
            enemyAgent.Warp(randomPoint);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Check if player is within chase range
        if (Vector3.Distance(_playerWhenSeen.position, transform.position) < enemyChaseRange! && Physics.Linecast(transform.position, _playerWhenSeen.position, out _lookForWalls))
        {
            isChasing = true;
            enemyAgent.speed = enemyChaseSpeed;
            enemyAnimator.SetBool("Walk", false);
            enemyAnimator.SetBool("Run", true);
            enemyAgent.SetDestination(_playerWhenSeen.position);
        }
        else
        {
            // Check if enemy is done moving
            if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance && !enemyAgent.pathPending)
            {
                enemyAnimator.SetBool("Walk", true);
                enemyAnimator.SetBool("Run", false);
                // Only move if not already chasing
                if (!isChasing && !waitNow)
                {
                    if (RandomPoint(centrePoint.position, enemyRange, out waitPoint))
                    {
                        Debug.DrawRay(waitPoint, Vector3.up, Color.blue, 1.0f);
                        enemyAnimator.SetBool("Walk", true);
                        enemyAnimator.SetBool("Run", false);
                        enemyAgent.SetDestination(waitPoint);
                        enemyAgent.speed = 3.5f;
                        waitNow = true;
                    }
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, waitPoint) < 0.1f)
                {
                    enemyAnimator.SetBool("Walk", false);
                    enemyAnimator.SetBool("LookAround", true);
                    waitNow = false;
                }
                isChasing = false;
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
