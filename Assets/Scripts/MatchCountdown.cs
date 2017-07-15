using UnityEngine.UI;
using UnityEngine;

public class MatchCountdown : MonoBehaviour
{
    public static MatchCountdown Instance
    {
        get;
        private set;
    }

    [SerializeField]
    private Text timerTxt;
    private float matchTime = 90;
    private bool countdown = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (!GameInfo.Instance.TimerAtCenter)
        {
            timerTxt.fontSize = 30;
            timerTxt.alignment = TextAnchor.UpperCenter;
        }
    }

	private void Update () {
        matchTime -= GameTime.TimeScale;
        if (countdown)
            if(timerTxt != null)
                timerTxt.text = ((int)matchTime).ToString();
	}

    public void SetMatchTime(float value)
    {
        matchTime = value;
        timerTxt.text = value.ToString();
    }

    public void SetText(string text)
    {
        timerTxt.text = text;
        EnableCountdown(false);
    }

    public void ShowText(string text, float time)
    {
        if (!this.enabled)
            return;

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
