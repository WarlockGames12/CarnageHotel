using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Movement: ")]
    [SerializeField] [Range(0, 100)] private float playerSpeed;
    [SerializeField] [Range(-10, 10)] private float playerGravity;

    //FOV Settings:
    [SerializeField] private AudioSource zoominOut;
    private float zoomSpeed = 10f;
    private float minZoom = 10f;
    private float maxZoom = 30f;
    private float zoomedInFOV = 20f;
    private float zoomedOutFOV = 60f;
    private float originalFOV;

    [Header("Sprint Settings: ")]
    [SerializeField] [Range(0,15)] private float sprintDuration;
    [SerializeField] [Range(0,10)] private float sprintCooldown;
    private float _timeLeft;
    private float minSpeed = 10f;
    private float maxSpeed = 20f;

    [Header("Step Sound Settings: ")] 
    [SerializeField] private AudioSource stepSound;
    [SerializeField] private AudioClip[] stepSounds;
    [SerializeField] private AudioSource jumpScareSound;

    [Header("Press E: ")]
    [SerializeField] private GameObject pressE;

    public static bool pressEs;
    [SerializeField] private bool isBeginning;
    
    
    [Header("Bopcurve")] 
    private AnimationCurve _animateCurve;
    [SerializeField] private Animator playOnly;
    
    /*[Header("Dialogue is Coming: ")] 
    [SerializeField] private GameObject textDialogue;
    [SerializeField] private DialogueManager dialogue;*/

    [SerializeField] private GameObject pauseMenu;
    private bool isPaused = false;
    
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
    
    public bool jumpScare = false;
    public bool isZoomedIn = false;

    // Start is called before the first frame update
    private void Start()
    {
        _animateCurve = new AnimationCurve();
        _animateCurve.AddKey(0, 0f);
        _animateCurve.AddKey(0.15f, 0.3f);
        _animateCurve.AddKey(0.3f, 0f);
        
        pauseMenu.SetActive(false);
        originalFOV = Camera.main.fieldOfView;
        
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

        if (Input.GetKeyDown(KeyCode.E) && TurnOn.isInRange)
            pressEs = true;
        else if (Input.GetKeyDown(KeyCode.E) && TurnOn.isInRange && pressEs)
            pressEs = false;

        if (Input.GetMouseButtonDown(1))
        {
            if (!isZoomedIn && !zoominOut.isPlaying)
            {
                zoominOut.pitch = Random.Range(0.7f, 1.2f);
                zoominOut.Play();
                ZoomIn();
            }
            else if (isZoomedIn && !zoominOut.isPlaying)
            {
                zoominOut.pitch = Random.Range(0.7f, 1.2f);
                zoominOut.Play();
                ZoomOut();
            }
        }
        SprintFeature();
        PauseGame();
    }

    private void ZoomIn()
    {
        isZoomedIn = true;
        StartCoroutine(ZoomTo(zoomedInFOV, 1f));
    }

    private void ZoomOut()
    {
        isZoomedIn = false;
        StartCoroutine(ZoomTo(zoomedOutFOV, 1f));
    }

    private IEnumerator ZoomTo(float targetZoom, float duration)
    {
        var oldZoom = Camera.main.fieldOfView;
        var t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            var newZoom = Mathf.Lerp(oldZoom, targetZoom, t / duration);
            Camera.main.fieldOfView = newZoom;
            yield return null;
        }
        Camera.main.fieldOfView = targetZoom;
    }
    
    private void SprintFeature()
    {
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

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
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
        if (!jumpScare)
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pressE == null)
            return;
        if (other.gameObject.CompareTag("Door") && !isBeginning)
        {
            pressE.SetActive(true);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            jumpScare = true;
            jumpScareSound.Play();
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
