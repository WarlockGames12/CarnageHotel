using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    
    [Header("JumpScare Incoming: ")]
    [SerializeField] private GameObject[] forCams;
    [SerializeField] private GameObject enemyIsGone;
    [SerializeField] private Animator jumpAni;
    [SerializeField] private GameObject gameOver;

    private PlayerMovement _getJumpScareGo;

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
        }
    }
}
