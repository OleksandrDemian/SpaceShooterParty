using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private static float timeModifier = 1f;
    private float timeScaleTarget = 1f;
    private static float time = 0;
    private List<Timer> timers = new List<Timer>();

    public static float TimeScale
    {
        get
        {
            return timeModifier * Time.deltaTime;
        }
        set
        {
            timeModifier = value;
        }
    }

    public static GameTime Instance {
        get;
        private set;
    }

    //GetTime not Time perche Time esiste di già in TimeScale.get{}
    public static float GetTime()
    {
        return time;
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        timeModifier = Mathf.LerpUnclamped(timeModifier, timeScaleTarget, Time.deltaTime*4);
        time += TimeScale;
        CheckTimers();
    }

    public void SetTimeScaleTarget(float target)
    {
        timeScaleTarget = target;
        //StartCoroutine(LerpTime(target));
    }

    public void AddTimer(Timer timer)
    {
        timers.Add(timer);
    }

    public void RemoveTimer(Timer timer)
    {
        timers.Remove(timer);
    }

    public void SetTimeScale(float target)
    {
        timeScaleTarget = target;
        timeModifier = target;
    }
    
    private void CheckTimers()
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            if (timers[i].Check(time))
            {
                timers.Remove(timers[i]);
            }
        }
    }
}