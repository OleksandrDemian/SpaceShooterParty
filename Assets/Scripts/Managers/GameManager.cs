using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(ImagePool))]
[RequireComponent(typeof(GameTime))]
[RequireComponent(typeof(MatchCountdown))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    public static ObjectPool ObjectPooler
    {
        get;
        private set;
    }

    public static ImagePool ImagePooler
    {
        get;
        private set;
    }

    //LevelManager
    private Vector2 mapBounds = new Vector2(22, 13);
    private List<Player> players;
    private Transform[] startPoints;
    private int MATCH_DURATION = 90; //JAVA syntax (maybe)
    private GameInfo gameInfo;
    private MatchCountdown countdown;
    private Timer matchTimer;
    //TEMP
    private bool matchEnded = false;

    //LevelManager
    public void CheckPosition(Transform transform)
    {
        Vector3 position = transform.position;
        if (Mathf.Abs(transform.position.x) > mapBounds.x)
        {
            if (position.x > 0)
                position.x = -mapBounds.x;
            else
                position.x = mapBounds.x;
        }
            
        if (Mathf.Abs(transform.position.y) > mapBounds.y)
        {
            if (position.y > 0)
                position.y = -mapBounds.y;
            else
                position.y = mapBounds.y;
        }
        transform.position = position;
    }

	private void Start ()
    {
        Instance = this;
        ObjectPooler = GetComponent<ObjectPool>();
        ImagePooler = GetComponent<ImagePool>();

        gameInfo = GameInfo.Instance;
        players = GetPlayers();
        startPoints = GetStartPoints();
        //MatchTime = gameInfo.gameTime;
        MATCH_DURATION = gameInfo.gameTime;

        countdown = GetComponent<MatchCountdown>();
        countdown.SetMatchTime(MATCH_DURATION);
        countdown.ShowText("Match started", 3);

        //Debug.Log("Time: " + MatchTime + " <> " + gameInfo.gameTime);
        GetComponent<AsteroidsGenerator>().enabled = gameInfo.enableAsteroids;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetRespawnPoint(startPoints[i]);
            players[i].InitializeShip(i);
            players[i].onConnectionClose = OnPlayerConnectionClose;
        }

        //StartCoroutine(StartDeelay(3));
        //StartCoroutine(MatchTimer(MATCH_DURATION + 3));
        StartMatch ();
    }

    private void OnPlayerConnectionClose(Player player)
    {
        //Check how match players there are!
        countdown.ShowText(player.Name + " left!", 4);
        Debug.Log("Players: " + EnabledPlayersCount);
        if (EnabledPlayersCount < 1)
        {
            GameTime.Instance.RemoveTimer(matchTimer);
            MatchEnd();
        }
    }

    private int EnabledPlayersCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < players.Count; i++)
            {
                Debug.Log("Player " + players[i].Name + " enabled: " + players[i].enabled);
                if (players[i].enabled)
                    counter++;
            }
            return counter;
        }
    }

    private void StartMatch()
    {
        matchTimer = new Timer(MATCH_DURATION, MatchEnd);
        GameTime.Instance.SetTimeScale(0.05f);
        GameTime.Instance.SetTimeScaleTarget(1);
        GameTime.Instance.AddTimer(matchTimer);
        Debug.Log("Durtion: " + MATCH_DURATION);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnableControll(true);
        }
    }

    /*private IEnumerator StartDeelay(int seconds)
    {
        GameTime.Instance.SetTimeScale(0.05f);
        for (int i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(seconds - i);
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnableControll(true);
        }
        GameTime.Instance.SetTimeScaleTarget(1);
    }*/
	
	private void Update ()
    {

	}

    /*private void OnGUI()
    {
        for(int i = 0; i < players.Count; i++)
            GUI.Label(new Rect(10, 10 + (20 * i), 150, 20), (players[i].Name + " -> " + players[i].kill + ": " + players[i].dead));
    }*/

    public List<Player> GetPlayers()
    {
        List<Player> temp = new List<Player>();

        foreach (Player player in GameObject.FindObjectsOfType<Player>())
        {
            temp.Add(player);
        }

        return temp;
    }

    private Transform[] GetStartPoints(){
        GameObject[] points = GameObject.FindGameObjectsWithTag("Respawn");
        Transform[] pointsTransform = new Transform[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            pointsTransform[i] = points[i].transform;
        }
        return pointsTransform;
    }

    public void LoadLobbyScreen()
    {
        foreach (Player p in players)
            Destroy(p.gameObject);
        SceneLoader.LoadScene("Lobby");
    }

    /*private IEnumerator MatchTimer(float time)
    {
        while (MatchTime < time)
        {
            MatchTime += GameTime.TimeScale;
            yield return new WaitForEndOfFrame();
        }
        MatchEnd();
    }*/

    private void MatchEnd()
    {
        if (matchEnded)
            return;

        foreach (Player player in players)
        {
            //player.Write(Converter.toString(Request.ADDPOINT));
            player.Write(Converter.toString(Request.MATCHEND));
            player.Input.ClearCommands();
            Debug.Log("Send to: " + player.Name + " -> " + Converter.toString(Request.ADDPOINT));
        }
        countdown.SetText("Match ended");
        countdown.enabled = false;
        GameTime.Instance.SetTimeScaleTarget(0.04f);
        matchEnded = true;
        OpenMatchResult();
    }

    private void OpenMatchResult()
    {
        Debug.Log("Show result");
        GameResultPanel result = FindObjectOfType<GameResultPanel>();
        result.Show(players.ToArray());
    }
}