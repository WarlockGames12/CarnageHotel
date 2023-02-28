 using System.Collections;
 using System.Security.Cryptography.X509Certificates;
 using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Enemy
{
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
    
        
        private Transform _playerTransform;
        private RaycastHit _lookForWalls;
        private Vector3 _waitPoint;
        private float _timeSinceLastSighting;

        private float _maxSpeed = 2;
        private string _animatorBlend = "Blend";

        private float blendTo;
        private float transitionDuration = 0.3f;
        private bool isTransitioning = false;
        
        private bool isWalking = false;
        private bool _isChasing;
        private bool _isLookingAround;
        private bool _isWaiting;

        // Start is called before the first frame update
        private void Start()
        {
            enemyAgent = GetComponent<NavMeshAgent>();
            _playerTransform = GameObject.FindWithTag("Player").transform;
            
            blendTo = 0f;
            _isWaiting = true;
            _isLookingAround = true;
            isWalking = false;
            _isChasing = false;
            enemyAnimator.GetFloat(_animatorBlend);
            blendTo = Animator.StringToHash(_animatorBlend);

            if (RandomPoint(centrePoint.position, enemyRange, out var randomPoint))
            {
                enemyAgent.Warp(randomPoint);
            }
            PlaySounds();
        }

        // Update is called once per frame
       private void Update()
       {
            EnemyMoves();
       }
        
        private void PlaySounds()
        {
            CancelInvoke();
            if (Vector3.Distance(_playerTransform.position, transform.position) < enemyChaseRange &&
                Physics.Linecast(transform.position, _playerTransform.position, out _lookForWalls) &&
                _lookForWalls.transform.CompareTag("Player") && !enemySteps.isPlaying)
            {
                WalkSounds(0.4f);
            } 
            else if (enemyAgent.velocity.magnitude > 0.1f && !enemySteps.isPlaying)
            {
                WalkSounds(0.8f);
            }
            else
            {
                InvokeRepeating(nameof(PlaySounds), 0,0.1f);
            }
        }
    
        private void WalkSounds(float delay)
        {
            //Play random step sound
            enemySteps.clip = stepSounds[Random.Range(0, stepSounds.Length)];
            enemySteps.pitch = Random.Range(0.8f, 1.2f);
            enemySteps.Play();
            Invoke(nameof(PlaySounds), delay);
        }

        private void EnemyMoves()
        {
            blendTo = 0f;
            if (Vector3.Distance(_playerTransform.position, transform.position) < enemyChaseRange && Physics.Linecast(transform.position, _playerTransform.position, out _lookForWalls) && _lookForWalls.transform.CompareTag("Player"))
            {
                if (_isLookingAround)
                {
                    _isWaiting = false;
                    _isLookingAround = false;
                    isWalking = false;
                    _isChasing = true;
                    blendTo = 1f;
                }
                else if (isWalking)
                {
                    _isWaiting = false;
                    _isLookingAround = false;
                    isWalking = false;
                    _isChasing = true;
                    blendTo = 1f;
                }
                _timeSinceLastSighting = 0f;
                enemyAgent.speed = 8f;
                enemyAgent.SetDestination(_playerTransform.position);
            }
            else
            {
                _timeSinceLastSighting += Time.deltaTime;

                // Check if enemy is done moving
                if (enemyAgent.remainingDistance <= enemyAgent.stoppingDistance && !enemyAgent.pathPending && !_isWaiting)
                {
                    if (_isChasing)
                    {
                        enemyAgent.speed = 2f;
                        isWalking = true;
                        _isChasing = false;
                        _isWaiting = false;
                        _isLookingAround = false;

                        if (RandomPoint(centrePoint.position, enemyRange, out _waitPoint))
                        {
                            enemyAgent.SetDestination(_waitPoint);
                            blendTo = 0f;
                            _isWaiting = true;
                            _isLookingAround = true;
                            isWalking = false;
                            _isChasing = false;
                        }
                    }
                    else if (!_isLookingAround && !_isWaiting)
                    {
                        blendTo = 0f;
                        enemyAgent.speed = 0f;
                        _isWaiting = true;
                        _isLookingAround = true;
                        isWalking = false;
                        _isChasing = false;
                    }
                }
                else if (!_isChasing && blendTo < 0.5f)
                {
                    blendTo = 0.5f;
                    enemyAgent.speed = 2f;
                    isWalking = true;
                    _isLookingAround = false;
                    _isWaiting = false;
                    _isChasing = false;
                } 
                
                if (_isWaiting && Vector3.Distance(transform.position, _waitPoint) < 0.1f)
                {
                    isWalking = true;
                    _isChasing = false;
                    _isWaiting = false;
                    _isLookingAround = false;
                    enemyAgent.speed = 2f;
                }

                switch (_timeSinceLastSighting > waitTime)
                { 
                    case true:
                    {
                        isWalking = true;
                        enemyAgent.speed = 2f;
                        _isLookingAround = false;
                        _isWaiting = false;
                        _isChasing = false;

                        if (RandomPoint(centrePoint.position, enemyRange, out _waitPoint))
                        {
                            enemyAgent.SetDestination(_waitPoint);
                            UpdateAnimatorBlend();
                            blendTo = 0f;
                            enemyAgent.speed = 0f;
                            _isLookingAround = true;
                            _isWaiting = true;
                            _timeSinceLastSighting = 0f;
                            isWalking = false;
                            _isChasing = false;
                        }
                        break;
                    }
                }
            }
            UpdateAnimatorBlend();
        }
    
        private void UpdateAnimatorBlend()
        {
            var targetBlend = blendTo;

            switch (enemyAgent.speed)
            {
                case 8f:
                    targetBlend = 1f;
                    _isChasing = true;
                    isWalking = false;
                    _isWaiting = false;
                    _isLookingAround = false;
                    break;
                case 2f:
                    targetBlend = 0.5f;
                    _isChasing = false;
                    isWalking = true;
                    _isWaiting = false;
                    _isLookingAround = false;
                    break;
                case 0f:
                    targetBlend = 0f;
                    _isChasing = false;
                    isWalking = false;
                    _isWaiting = false;
                    _isLookingAround = true;
                    break;
            }
            

            // Smoothly transition the blend value
            blendTo = Mathf.MoveTowards(enemyAnimator.GetFloat(_animatorBlend), targetBlend, Time.deltaTime * _maxSpeed);

            // Check if a transition is needed
            if (!isTransitioning && Mathf.Abs(blendTo - enemyAnimator.GetFloat(_animatorBlend)) > 0.01f)
            {
                isTransitioning = true;
                StartCoroutine(BlendToAnimation(targetBlend));
            }
            else
            {
                enemyAnimator.SetFloat(_animatorBlend, blendTo);
            }
        }

        private IEnumerator BlendToAnimation(float targetBlend)
        {
            if (isTransitioning) yield break;
            isTransitioning = true;

            var initialBlend = enemyAnimator.GetFloat(_animatorBlend);

            var elapsedTime = 0f;
            while (elapsedTime < transitionDuration)
            {
                var blend = Mathf.Lerp(initialBlend, targetBlend, elapsedTime / transitionDuration);
                enemyAnimator.SetFloat(_animatorBlend, blend);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (_isChasing)
            {
                blendTo = 1f;
                enemyAnimator.CrossFade("Run", transitionDuration);
            }
            else if (isWalking)
            {
                blendTo = 0.5f;
                enemyAnimator.CrossFade("Walk", transitionDuration);
            }
            else if (_isLookingAround || _isWaiting)
            {
                blendTo = 0f;
                enemyAnimator.CrossFade("LookAround", transitionDuration);
            }

            isTransitioning = false;
        }

        
        private static bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            var randomPoint = center + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }
    }
}