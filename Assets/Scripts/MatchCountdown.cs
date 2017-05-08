using UnityEngine.UI;
using UnityEngine;

public class MatchCountdown : MonoBehaviour {

    [SerializeField]
    private Text timerTxt;

    private float matchTime = 90;
    private bool countdown = true;
	
	void Update () {
        matchTime -= GameTime.TimeScale;
        if(countdown)
            timerTxt.text = "Time left: " + (int)matchTime;
	}

    public void SetMatchTime(float value)
    {
        matchTime = value;
        timerTxt.text = "Time left: " + value;
    }

    public void SetText(string text)
    {
        timerTxt.text = text;
        EnableCountdown(false);
    }

    public void ShowText(string text, float time)
    {
        timerTxt.text = text;
        EnableCountdown(false);
        GameTime.Instance.AddTimer(new Timer(time, delegate()
        {
            EnableCountdown(true);
        }));
    }

    private void EnableCountdown(bool action)
    {
        countdown = action;
    }
}
