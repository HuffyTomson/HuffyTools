using UnityEngine;
using System.Collections;
using Huffy.Utilities;

public class GameManager : MonoBehaviour
{
    StateMachine sm;
    State stateOne;
    State stateTwo;

    IEnumerator Start ()
    {
        VersionNumber.Initialize();
        Config.Initialize();

        yield return new WaitForSeconds(0.125f);
        Container.Load("Container").PrintString();

        stateOne = new State(UpdateOne, EnterOne, ExitOne);
        stateTwo = new State(UpdateTwo, EnterTwo, ExitTwo);
        sm = new StateMachine(stateOne);
    }

    void Update()
    {
        if (sm != null)
            sm.Update();
    }

    void UpdateOne()
    {
        if (Input.GetKeyDown("2"))
            sm.ChangeState(stateTwo);
    }

    void UpdateTwo()
    {
        if (Input.GetKeyDown("1"))
            sm.ChangeState(stateOne);
    }

    void EnterOne()
    {
        Debug.Log("EnterOne");
    }
    void ExitOne()
    {
        Debug.Log("ExitOne");
    }
    void EnterTwo()
    {
        Debug.Log("EnterTwo");
    }
    void ExitTwo()
    {
        Debug.Log("ExitTwo");
    }
}
