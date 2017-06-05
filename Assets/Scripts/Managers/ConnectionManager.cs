using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    private Server server;
    private List<Player> players = new List<Player>();
    private float lastConnectionCheck = 0f;

    public static ConnectionManager Instance
    {
        get;
        private set;
    }

    public static string GetLocalIP()
    {
        return Network.player.ipAddress;
    }

    public void InstantiatePlayer(ConnectionReader reader)
    {
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        Player player = newPlayer.GetComponent<Player>();
        player.onConnectionClose = OnConnectionClose;
        player.SetConnectionReader(reader);
        reader.initialized = true;
        players.Add(player);
    }

    private void Start()
    {
        Instance = this;
        Debug.Log("Old IP: " + Server.GetLocalIPAddress());
        server = new Server(6546);
    }
	
	private void Update ()
    {
        server.AddNewConnection();
        if (Time.time > lastConnectionCheck + 2)
        {
            CheckConnections();
            lastConnectionCheck = Time.time;
        }
	}

    private void CheckConnections()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Write("");
        }
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }

    private void OnConnectionClose(Player player)
    {
        players.Remove(player);
        server.RemoveConnection(player.Connection);
        LobbyManager.Instance.RemoveName(player.Name);
        Destroy(player.gameObject);
    }
}
