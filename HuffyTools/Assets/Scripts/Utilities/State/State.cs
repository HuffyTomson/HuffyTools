using UnityEngine;
using System.Collections;
using System;

public class State
{
    Action update;
    Action enter;
    Action exit;

    public Action Update { get { return update; } }
    public Action Enter { get { return enter; } }
    public Action Exit { get { return exit; } }

    public State(Action _update, Action _enter = null, Action _exit = null)
    {
        update = _update;
        enter = _enter;
        exit = _exit;
    }
    
}
