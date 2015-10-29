using UnityEngine;
using System.Collections;

public class StateMachine
{
    private State currentState;
    private State previousState;

    public StateMachine(State _startState)
    {
        ChangeState(_startState);
    }

    public void ChangeState(State _nextState)
    {
        if(currentState != null)
        {
            if (currentState.Exit != null)
                currentState.Exit();
        }

        previousState = currentState;
        currentState = _nextState;

        if(currentState != null)
        {
            if (currentState.Enter != null)
                currentState.Enter();
        }
    }

    public void RevertState()
    {
        ChangeState(previousState);
    }

    public void Update()
    {
        if (currentState != null)
        {
            if (currentState.Update != null)
                currentState.Update();
        }
    }
}
