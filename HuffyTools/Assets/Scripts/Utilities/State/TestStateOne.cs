using UnityEngine;
using System.Collections;
using System;

public class TestStateOne: State<GameManager>
{
    public TestStateOne(GameManager _owner) : base(_owner)
    {
    }

    public override void Enter()
    {
        owner.id++;
        Debug.Log("One Enter" + owner.id);
    }

    public override void Exit()
    {
        Debug.Log("One Exit");
    }

    public override void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("One to One");
            sm.ChangeState(new TestStateOne(owner));
        }
        if (Input.GetKeyDown("2"))
        {
            Debug.Log("One to Two");
            sm.ChangeState(new TestStateTwo(owner));
        }
    }
}
