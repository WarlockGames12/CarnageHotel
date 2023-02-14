using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Wandering,
    Alert,
    Chase
}

public class StateMachine : MonoBehaviour
{
    private Dictionary<EnemyStates, State> _enemyState;
    private State _currentState;

    // Update is called once per frame
    private void Update()
    {
        if (_currentState == null)
            return;

        _currentState.Reason();
        _currentState.Act();
    }

    public void SetState(EnemyStates stateID)
    {
        if (!_enemyState.ContainsKey(stateID))
            return;

        if (_currentState != null)
            _currentState.Leave();

        _currentState = _enemyState[stateID];
        _currentState.Enter();
    }

    public void AddState(EnemyStates stateID, State state)
    {
        _enemyState.Add(stateID, state);
    }
}
