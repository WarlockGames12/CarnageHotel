using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    
    [Header("JumpScare Incoming: ")]
    [SerializeField] private GameObject[] forCams;
    [SerializeField] private GameObject enemyIsGone;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private Animator jumpAni;
    [SerializeField] private Animator gameOverButtonsAni;
    [SerializeField] private AudioSource gameOverClip;

    private PlayerMovement _getJumpScareGo;
    private bool _hasPlayedGameOver = false;
    private bool _getButtons = false;

    private void Start()
    {
        _getJumpScareGo = GetComponent<PlayerMovement>();
        enemyIsGone.SetActive(true);
        foreach (var t in forCams)
        {
            t.SetActive(false);
        }
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_getJumpScareGo.jumpScare)
        {
            foreach (var t in forCams)
            {
                t.SetActive(true);
            }
            enemyIsGone.SetActive(false);
            if (jumpAni.GetCurrentAnimatorStateInfo(0).IsName("JumpPunch") && !_hasPlayedGameOver)
            {
                _hasPlayedGameOver = true;
                Invoke("ShowGameOverUI", 1f);
            }
        }
    }

    private void ShowGameOverUI()
    {
        gameOver.SetActive(true);
        Invoke("ShowButtons", 1f);
    }

    private void ShowButtons()
    {
        gameOverClip.Play();
        gameOverButtonsAni.SetTrigger("Ani");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
