using UnityEngine;

public class ConnectionManager : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab;
    private Server server;

    public static ConnectionManager Instance
    {
        get;
        private set;
    }

    public void InstantiatePlayer(ConnectionReader reader)
    {
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        Player player = newPlayer.GetComponent<Player>();
        player.SetConnectionReader(reader);
        reader.initialized = true;
    }

    void Start()
    {
        Instance = this;
        server = new Server(6546);
    }
	
	void Update ()
    {
        server.AddNewConnection();
	}
}
