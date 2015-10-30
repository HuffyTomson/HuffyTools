using UnityEngine;
using System.Collections;
using System;

public class TestStateTwo : State<GameManager>
{
    public TestStateTwo(GameManager _owner) : base(_owner)
    {
    }

    public override void Enter()
    {
        owner.id--;
        Debug.Log("Two Enter" + owner.id);
    }

    public override void Exit()
    {
        Debug.Log("Two Exit");
    }

    public override void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            Debug.Log("Two to One");
            sm.ChangeState(new TestStateOne(owner));
        }
        if (Input.GetKeyDown("2"))
        {
            Debug.Log("Two to Two");
            sm.ChangeState(new TestStateTwo(owner));
        }
    }
}
