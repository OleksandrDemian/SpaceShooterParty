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

        StartCoroutine(StartDeelay());
	}

    private IEnumerator StartDeelay()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(3 - i);
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
            GUI.Label(new Rect(10, 10 + (20 * i), 100, 20), (players[i].Name + ": " + players[i].kill));
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
}
