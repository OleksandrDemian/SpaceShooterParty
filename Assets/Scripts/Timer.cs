public delegate void OnTimerTrigger();

public class Timer
{
    private float triggerTime;
    private OnTimerTrigger onTimerTrigger;

    public Timer(float time, OnTimerTrigger callback)
    {
        triggerTime = GameTime.GetTime() + time;
        onTimerTrigger = callback;
    }

    public bool Check(float time)
    {
        if (time > triggerTime)
        {
            onTimerTrigger();
            return true;
        }
        return false;
    }
}