﻿using System.Collections.Generic;
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
    public Vector2 MapBounds
    {
        get;
        private set;
    }

    private List<Player> players;
    private Transform[] startPoints;
    private int MATCH_DURATION = 60;
    private GameInfo gameInfo;
    private MatchCountdown countdown;
    private Timer matchTimer;
    private bool matchEnded = false;
    private bool matchInterrupted = false;

    private void Awake()
    {
        Instance = this;

        if (GameInfo.Instance != null)
            gameInfo = GameInfo.Instance;
        else
            gameInfo = new GameInfo();
    }

    private void Start()
    {
        ObjectPooler = GetComponent<ObjectPool>();
        ImagePooler = GetComponent<ImagePool>();

        players = GetPlayers();
        startPoints = GetStartPoints();
        //MatchTime = gameInfo.gameTime;
        MATCH_DURATION = gameInfo.MatchTime;

        Debug.Log("GameInfo: " + gameInfo.ToString());

        AdjustCameraSize(gameInfo.MapSize);

        AdjustStartPositions(gameInfo.MapSize);

        countdown = MatchCountdown.Instance;
        countdown.SetMatchTime(MATCH_DURATION);
        countdown.ShowText("Match started", 3);

        //Debug.Log("Time: " + MatchTime + " <> " + gameInfo.gameTime);
        GetComponent<AsteroidsGenerator>().enabled = gameInfo.AsteroidsEnabled;

        if (gameInfo.BonusesEnbled)
        {
            BonusGenerator bonusGenerator = new BonusGenerator();
        }

        //Debug.Log("Bonuses: " + gameInfo.enableBonuses);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetRespawnPoint(startPoints[i]);
            players[i].InitializeShip(i);
            players[i].onConnectionClose = OnPlayerConnectionClose;
        }

        if (PauseScreen.Instance != null)
        {
            PauseScreen.Instance.AddOnPauseEvent(delegate ()
            {
                GameTime.Instance.SetTimeScale(0f);
                Cursor.visible = true;
            });

            PauseScreen.Instance.AddOnResumeEvent(delegate ()
            {
                GameTime.Instance.SetTimeScaleTarget(1f);
                Cursor.visible = matchEnded;
            });
        }

        StartMatch();
    }

    private void AdjustCameraSize(int size)
    {
        if (size < 8)
        {
            Debug.LogError("Map size is wrong!!!");
            size = 12;
        }

        Camera.main.orthographicSize = size;
        MapBounds = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        MapBounds += new Vector2(.3f, .3f);
    }

    private void AdjustStartPositions(float mapSize)
    {
        float value = mapSize / 12;

        for (int i = 0; i < startPoints.Length; i++)
        {
            startPoints[i].position *= value;
        }
    }

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

    private void OnPlayerConnectionClose(Player player)
    {
        countdown.ShowText(player.Name + " left!", 3);

        if (EnabledPlayersCount < 2)
        {
            GameTime.Instance.RemoveTimer(matchTimer);
            InterruptMatch();
        }
    }
    
    public void InterruptMatch()
    {
        matchInterrupted = true;
        MatchEnd();
    }
    
    private int EnabledPlayersCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < players.Count; i++)
            {
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

        Cursor.visible = false;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnableControll(true);
        }
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
        {
            p.CloseConnection();
            Destroy(p.gameObject);
        }
        SceneLoader.LoadScene("Lobby");
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        for (int i = 0; i < players.Count; i++)
        {
            //GUI.Label(new Rect(10, 15 + (15 * i), 100, 20), players[i].Name + ": " + players[i].kill);
        }
    }
#endif

    public void MatchEnd()
    {
        if (matchEnded)
            return;

        OpenMatchResult();

        foreach (Player player in players)
        {
            player.Input.ClearCommands();
            player.Write(Command.MATCHEND);
        }

        countdown.SetText("Match ended");
        countdown.enabled = false;

        GameTime.Instance.SetTimeScaleTarget(0.04f);
        matchEnded = true;
    }

    public Vector3 GetRandomPoint()
    {
        return new Vector3(Random.Range(-MapBounds.x, MapBounds.x), Random.Range(-MapBounds.y, MapBounds.y), 0) * 0.85f;
    }

    private void OpenMatchResult()
    {
        if (PauseScreen.Instance.IsPaused())
            PauseScreen.Instance.PauseTrigger();

        Cursor.visible = true;

        if (players.Count > 0)
        {
            Player[] playersOrderedList = players.ToArray();
            System.Array.Sort(playersOrderedList, (y, x) => x.GetStatistic().KillCounter().CompareTo(y.GetStatistic().KillCounter()));

            if(!matchInterrupted)
                playersOrderedList[0].Write(Command.ADDPOINT);

            GameResultPanel result = FindObjectOfType<GameResultPanel>();
            result.Show(playersOrderedList);
        }
        else
        {
            SceneLoader.LoadScene("Lobby");
        }
    }
}