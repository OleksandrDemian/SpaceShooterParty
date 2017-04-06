﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(ImagePool))]
[RequireComponent(typeof(GameTime))]
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
        //Debug.Log("Time: " + MatchTime + " <> " + gameInfo.gameTime);
        if (!gameInfo.enableAsteroids)
            GetComponent<AsteroidsGenerator>().enabled = false;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].SetRespawnPoint(startPoints[i]);
            players[i].InitializeShip(i);
            players[i].EnableControll(false);
        }

        //StartCoroutine(StartDeelay(3));
        //StartCoroutine(MatchTimer(MATCH_DURATION + 3));
        StartMatch ();
    }

    private void StartMatch()
    {
        GameTime.Instance.SetTimeScale(0.05f);
        GameTime.Instance.SetTimeScaleTarget(1);
        GameTime.Instance.AddTimer(new Timer(MATCH_DURATION, MatchEnd));
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

    public void LoadLobbyScreen()
    {
        foreach (Player p in players)
            Destroy(p.gameObject);
        SceneLoader.LoadScene("Lobby");
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
        foreach (Player player in players)
        {
            //player.Write(Converter.toString(Request.ADDPOINT));
            player.Write(Converter.toString(Request.MATCHEND));
            Debug.Log("Send to: " + player.Name + " -> " + Converter.toString(Request.ADDPOINT));
        }
        GameTime.Instance.SetTimeScaleTarget(0.04f);
        OpenMatchResult();
    }

    private void OpenMatchResult()
    {
        Debug.Log("Show result");
        GameResultPanel result = FindObjectOfType<GameResultPanel>();
        result.Show(players.ToArray());
    }
}