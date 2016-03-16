using UnityEngine;
using System.Collections;
using System;

public abstract class State<T>
{
    public T owner;
    public StateMachine<T> sm;

    public State(T _owner)
    {
        owner = _owner;
    }

    abstract public void Update();
    abstract public void Enter();
    abstract public void Exit();
}
