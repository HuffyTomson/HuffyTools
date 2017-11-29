using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Huffy.Utilities;

public class TimeManager : SingletonBehaviour<TimeManager>
{
    [SerializeField]
    private int stopTimeCount = 0;
    private float timeScale = 1;
    float previousTimeSinceStartup;
    public static float unscaledDeltaTime { get; private set; }
    public static float deltaTime { get { return Time.deltaTime; } }

    void Awake()
    {
        previousTimeSinceStartup = Time.realtimeSinceStartup;
    }

    void Update()
    {
        float realtimeSinceStartup = Time.realtimeSinceStartup;
        unscaledDeltaTime = realtimeSinceStartup - previousTimeSinceStartup;
        previousTimeSinceStartup = realtimeSinceStartup;

        if (unscaledDeltaTime < 0)
            unscaledDeltaTime = 0;
    }

    public static void Reset()
    {
        Instance.stopTimeCount = 0;
        Instance.timeScale = 1;
        Time.timeScale = Instance.timeScale;
    }

    public static bool TimeStpped
    {
        get { return Instance.stopTimeCount > 0; }
    }

    public static void SetTimeScale(float _timeScale)
    {
        Instance.timeScale = _timeScale;
    }

    public static void StopTime()
    {
        Instance.stopTimeCount++;

        if (Instance.stopTimeCount > 0)
        {
            Time.timeScale = 0;
            //PlayerInput.SetInputActive(false);
        }
    }

    public static void StartTime()
    {
        Instance.stopTimeCount--;

        if(Instance.stopTimeCount < 0)
            Instance.stopTimeCount = 0;

        if (Instance.stopTimeCount == 0)
        {
            Time.timeScale = Instance.timeScale;
            //PlayerInput.SetInputActive(true);
        }
    }
}
