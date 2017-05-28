using UnityEngine;

public delegate void PauseEvent();

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen Instance
    {
        get;
        private set;
    }

    private GameObject pauseScreen;
    private bool isPaused = false;
    private event PauseEvent onPause;
    private event PauseEvent onResume;

    private void Awake()
    {
        Instance = this;
    }

	private void Start ()
    {
        pauseScreen = transform.FindChild("ScreenOverlay").gameObject;
        GetComponent<Canvas>().worldCamera = Camera.main;
        isPaused = false;
        pauseScreen.SetActive(false);
    }
	
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseTrigger();
        }
	}

    public void PauseTrigger()
    {
        isPaused = !isPaused;
        Debug.Log(isPaused);
        if (isPaused)
        {
            if(onPause != null)
                onPause();
            pauseScreen.SetActive(true);
        }
        else
        {
            if (onResume != null)
                onResume();
            pauseScreen.SetActive(false);
        }
    }

    public void AddOnPauseEvent(PauseEvent onPause)
    {
        this.onPause += onPause;
    }

    public void AddOnResumeEvent(PauseEvent onResume)
    {
        this.onResume += onResume;
    }
}
