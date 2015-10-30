using UnityEngine;
using System.Collections;

public class StateMachine<T>
{
    private State<T> currentState;
    private State<T> previousState;

    public StateMachine(State<T> _startState)
    {
        ChangeState(_startState);
    }

    public void ChangeState(State<T> _nextState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        previousState = currentState;
        currentState = _nextState;
        
        if(currentState != null)
        {
            currentState.sm = this;
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
            currentState.Update();
        }
    }
}
