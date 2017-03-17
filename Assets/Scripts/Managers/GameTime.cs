/*using System.Collections;
using System.Collections.Generic;*/
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private static float timeModifier = 1f;
    private float timeScaleTarget = 1f;

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

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        timeModifier = Mathf.LerpUnclamped(timeModifier, timeScaleTarget, Time.deltaTime);
    }

    public void SetTimeScaleTarget(float target)
    {
        timeScaleTarget = target;
        //StartCoroutine(LerpTime(target));
    }

    /*private IEnumerator LerpTime(float targetTimeScale)
    {
        float speed = Time.deltaTime;
        while (timeModifier != targetTimeScale)
        {
            timeModifier = Mathf.LerpUnclamped(timeModifier, targetTimeScale, speed);
            speed += Time.deltaTime/4;

            yield return new WaitForEndOfFrame();
        }
    }*/
}