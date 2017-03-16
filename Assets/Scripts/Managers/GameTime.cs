using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
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

    private static float timeModifier = 1f;

    private void Start()
    {
        Instance = this;
    }

    public void SetTimeScaleTarget(float target)
    {
        StartCoroutine(LerpTime(target));
    }

    private IEnumerator LerpTime(float targetTimeScale)
    {
        float speed = Time.deltaTime;
        while (timeModifier != targetTimeScale)
        {
            timeModifier = Mathf.LerpUnclamped(timeModifier, targetTimeScale, speed);
            speed += Time.deltaTime/4;

            yield return new WaitForEndOfFrame();
        }
    }
}