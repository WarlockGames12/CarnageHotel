using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private Animator enemyAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Act()
    {
        throw new System.NotImplementedException();
    }

    public override void Reason()
    {
        throw new System.NotImplementedException();
    }
}
