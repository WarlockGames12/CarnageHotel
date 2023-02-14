using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Movement: ")]
    [SerializeField] [Range(0, 100)] private float playerSpeed;
    [SerializeField] [Range(-10, 10)] private float playerGravity;

    [Header("Sprint Settings: ")]
    [SerializeField] [Range(0,15)] private float sprintDuration;
    [SerializeField] [Range(0,10)] private float sprintCooldown;
    private float _timeLeft;
    private float minSpeed = 10f;
    private float maxSpeed = 20f;

    [Header("Step Sound Settings: ")] 
    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioClip[] stepSounds;

    [Header("Press E: ")]
    [SerializeField] private GameObject pressE;
    [SerializeField] private bool isBeginning;
    
    
    [Header("Bopcurve")] 
    private AnimationCurve _animateCurve;
    [SerializeField] private Animator playOnly;
    
    /*[Header("Dialogue is Coming: ")] 
    [SerializeField] private GameObject textDialogue;
    [SerializeField] private DialogueManager dialogue;*/
    
    [Header("Flashlight Settings: ")] 
    [SerializeField] private AudioSource flashLightSound;
    [SerializeField] private GameObject flashLight;
    private bool isLit = true;
    
    // private things that is needed
    private CharacterController _playerController;
    
    // Player Movement Settings: 
    private float _moveX;
    private float _moveY;
    private float _stepDelay = 0.5f;
    private float _nextStepTime;
    private Vector3 _playerVelocity;

    // Start is called before the first frame update
    private void Start()
    {
        _animateCurve = new AnimationCurve();
        _animateCurve.AddKey(0, 0f);
        _animateCurve.AddKey(0.15f, 0.3f);
        _animateCurve.AddKey(0.3f, 0f);
        
        playOnly.enabled = false;
        _playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        if (Input.GetMouseButtonDown(0))
        {
            ToggleLight();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _timeLeft <= 0)
        {
            playerSpeed = minSpeed;
            _timeLeft = sprintDuration;
        }
        else if (_timeLeft > 0)
        {
            playerSpeed = maxSpeed;
            _timeLeft -= Time.deltaTime;
        }
        else if (_timeLeft <= 0 && playerSpeed == maxSpeed)
        {
            playerSpeed = minSpeed;
            _timeLeft = sprintCooldown;
        }
        else if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
        }
    }
    
    private void ToggleLight()
    {
        isLit = !isLit;
        flashLightSound.Play();
        flashLight.SetActive(isLit);
    }

    private void Move()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveY = Input.GetAxis("Vertical");

        var thisTransform = transform;
        var playerMoves = thisTransform.right * _moveX + thisTransform.forward * _moveY;
        playerSpeed = playerMoves.magnitude;
        
        playerSpeed = Mathf.Clamp(playerSpeed, minSpeed, maxSpeed);
        
        if (_moveX != 0 || _moveY != 0)
        {
            PlayStepSound();
            playOnly.enabled = true;
            var bop = _animateCurve.Evaluate(Time.time) * playerSpeed;
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
                playerSpeed = maxSpeed;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                playerSpeed = minSpeed;
            
            Debug.Log(playerSpeed);
            _playerController.Move(playerMoves * playerSpeed * Time.deltaTime + new Vector3(0,bop,0));
        }
        else
        {
            playOnly.enabled = false;
            _playerController.Move(playerMoves * playerSpeed * Time.deltaTime + new Vector3(0,0,0));
        }
        
        _playerVelocity.y += playerGravity * Time.deltaTime;
        _playerController.Move(_playerVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pressE == null)
            return;
        if (other.gameObject.CompareTag("Door") && !isBeginning)
        {
            pressE.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        pressE.SetActive(false);
    }

    private void PlayStepSound()
    {
        switch (Time.time > _nextStepTime)
        {
            case true:
            {
                _nextStepTime = Time.time + _stepDelay;
                var randomIndex = Random.Range(0, stepSounds.Length);
                stepSound.clip = stepSounds[randomIndex];
                stepSound.Play();
                break;
            }
        }
    }
}
