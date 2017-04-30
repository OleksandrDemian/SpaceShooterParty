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
    
    public Vector2 MapBounds
    {
        get;
        private set;
    }

    private List<Player> players;
    private Transform[] startPoints;
    private int MATCH_DURATION = 60; //JAVA syntax (maybe)
    private GameInfo gameInfo;
    private MatchCountdown countdown;
    private Timer matchTimer;
    //TEMP
    private bool matchEnded = false;
    BonusGenerator generator;

    //LevelManager
    public void CheckPosition(Transform transform)
    {
        Vector3 position = transform.position;
        if (Mathf.Abs(transform.position.x) > MapBounds.x)
        {
            if (position.x > 0)
                position.x = -MapBounds.x;
            else
                position.x = MapBounds.x;
        }
            
        if (Mathf.Abs(transform.position.y) > MapBounds.y)
        {
            if (position.y > 0)
                position.y = -MapBounds.y;
            else
                position.y = MapBounds.y;
        }
        transform.position = position;
    }

	private void Start ()
    {
        //NEW STUFF
        MapBounds = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        MapBounds += new Vector2(.5f, .5f);
        //NED NEW STUFF

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

        if (gameInfo.enableBonuses)
        {
            generator = new BonusGenerator();
        }

        //Debug.Log("Bonuses: " + gameInfo.enableBonuses);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetRespawnPoint(startPoints[i]);
            players[i].InitializeShip(i);
            players[i].onConnectionClose = OnPlayerConnectionClose;
        }

        StartMatch ();
    }

    //PLAYERS COUNT < 2!!!!!
    private void OnPlayerConnectionClose(Player player)
    {
        //Check how match players there are!
        countdown.ShowText(player.Name + " left!", 4);
        Debug.Log("Players: " + EnabledPlayersCount);
        //PLAYERS COUNT < 2!!!!!
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
	
	private void Update ()
    {

	}

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

    private void MatchEnd()
    {
        if (matchEnded)
            return;

        foreach (Player player in players)
        {
            player.Input.ClearCommands();
            //player.Write(Converter.toString(Request.ADDPOINT));
            player.Write(Converter.toString(Request.MATCHEND));
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