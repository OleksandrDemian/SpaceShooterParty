using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
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

    //LevelManager
    private Vector2 mapBounds = new Vector2(22, 13);
    private List<Player> players;

    //LevelManager
    public void CheckPosition(Transform transform)
    {
        Vector3 position = transform.position;
        if (Mathf.Abs(transform.position.x) > mapBounds.x)
            position.x *= -1;
        if (Mathf.Abs(transform.position.y) > mapBounds.y)
            position.y *= -1;
        transform.position = position;
    }

	private void Start ()
    {
        Instance = this;
        ObjectPooler = GetComponent<ObjectPool>();

        players = GetPlayers();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].InitializeShip(i);
            players[i].EnableControll(false);
        }

        StartCoroutine(StartDeelay(3));
        StartCoroutine(MatchTimer(120 + 3));
	}

    private IEnumerator StartDeelay(int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(seconds - i);
        }
        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnableControll(true);
        }
    }
	
	private void Update ()
    {

	}

    private void OnGUI()
    {
        for(int i = 0; i < players.Count; i++)
            GUI.Label(new Rect(10, 10 + (20 * i), 150, 20), (players[i].Name + " -> " + players[i].kill + ": " + players[i].dead));
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

    private IEnumerator MatchTimer(float time)
    {
        yield return new WaitForSeconds(time);
        MatchEnd();
    }

    private void MatchEnd()
    {
        foreach (Player player in players)
        {
            player.Write(Converter.toString(Request.ADDPOINT));
            player.Write(Converter.toString(Request.MATCHEND));
            Debug.Log("Send to: " + player.Name + " -> " + Converter.toString(Request.ADDPOINT));
        }
        Time.timeScale = 0.01f;
        OpenMatchResult();
    }

    private void OpenMatchResult()
    {
        Debug.Log("Show result");
    }
}