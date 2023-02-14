using UnityEngine;

public class WanderingState : State
{

    [SerializeField] private StateMachine _stateMachine;

    [Header("Wandering Settings: ")] 
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float hearingRange = 5f;
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderInterval = 5f;
    [SerializeField] private float wanderSpeed = 1f;
    [SerializeField] private Animator enemyAnimator;

    [Header("Obstacle Avoidance Settings: ")] 
    [SerializeField] private float raycastDistance = 2f;
    [SerializeField] private float raycastAngle = 30f;

    private Transform _sawPlayer;
    private Vector3 _lastKnownPlayerPos;
    private Vector3 _wanderTarget;
    private float _wanderTimer;
    private float _pathfindingTimer;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        _sawPlayer = GameObject.FindWithTag("Player").transform;
        _audioSource = GetComponent<AudioSource>();
        _wanderTarget = RandomWanderTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        if (CanHearPlayer())
        {
            _stateMachine.SetState(EnemyStates.Alert);
        }

        if (CanSeePlayer())
        {
            _stateMachine.SetState(EnemyStates.Alert);
        }

        _wanderTimer += Time.deltaTime;
        if (_wanderTimer >= wanderInterval)
        {
            _wanderTimer = 0f;
            _wanderTarget = RandomWanderTarget();
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, _sawPlayer.position) > sightRange)
            return false;

        RaycastHit hit;
        if (!Physics.Linecast(transform.position, _sawPlayer.position, out hit)) return false;
        _stateMachine.SetState(EnemyStates.Alert);
        return hit.transform == _sawPlayer;

    }

    private bool CanHearPlayer()
    {
        if (Vector3.Distance(transform.position, _sawPlayer.position) > hearingRange)
            return false;

        _stateMachine.SetState(EnemyStates.Alert);
        return _audioSource.isPlaying;
    }



    private Vector3 RandomWanderTarget()
    {
        var wanderTarget = transform.position + (Random.insideUnitSphere * wanderRadius);
        wanderTarget.y = transform.position.y;
        return wanderTarget;
    }
    
    public override void Act()
    {
        enemyAnimator.SetBool("Walk", true);
        transform.position = Vector3.MoveTowards(transform.position, _wanderTarget, wanderSpeed * Time.deltaTime);

        if (CanHearPlayer())
        {
            _stateMachine.SetState(EnemyStates.Alert);
        }
        if (CanSeePlayer())
        {
            _stateMachine.SetState(EnemyStates.Alert);
        }

        var forward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, raycastDistance))
        {
            // if is in obstacle range, change wander target to avoid it
            Debug.Log("I am here");
            var right = transform.TransformDirection(Vector3.right);
            var left = transform.TransformDirection(Vector3.left);
            var newTarget = _wanderTarget;

            if (!Physics.Raycast(transform.position, right, out hit, raycastDistance))
                newTarget += right * wanderRadius;
            else if (!Physics.Raycast(transform.position, left, out hit, raycastDistance))
                newTarget += left * wanderRadius;
            else
                newTarget = RandomWanderTarget();
            _wanderTarget = newTarget;
        }

        transform.position = Vector3.MoveTowards(transform.position, _wanderTarget, wanderSpeed * Time.deltaTime);
    }

    public override void Reason()
    {
        throw new System.NotImplementedException();
    }
}
