using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IJoystickListener
{
    private ConnectionReader reader;
    private ShipController ship;
    private bool controllEnabled = false;

    private string playerName = "Nameless";
    public int kill = 0;
    public int dead = 0;

    private void Start()
    {

    }

    private void Update()
    {
        reader.Read();
    }

    public void Write(string message) {
        reader.Write(message);
    }

    public void InitializeShip()
    {
        ship = GameManager.ObjectPooler.Get(GOType.SHIP).GetComponent<ShipController>();
        ship.SetPlayer(this);
    }

    public void EnableControll(bool action)
    {
        controllEnabled = action;
    }

    public void OnMessageRead(string message)
    {
        Debug.Log("Message: " + message);
        if (message[0] == 'c')
            ReadCommand(message);
        if (message[0] == 'r')
            ReadRequest(message);
    }

    public void SetConnectionReader(ConnectionReader reader)
    {
        this.reader = reader;
        reader.SetJoystickListener(this);
        DontDestroyOnLoad(gameObject);
    }

    public void Death()
    {
        dead++;
        reader.Write("Dead");
        ship.gameObject.SetActive(false);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
        ship.transform.position = Vector3.zero;
        ship.gameObject.SetActive(true);
    }

    public void Kill()
    {
        kill++;
        reader.Write("Kill");
    }

    public string Name
    {
        get { return playerName; }
    }

    public int ID
    {
        get { return reader.ID; }
    }

    private void ReadCommand(string message)
    {
        if (!controllEnabled)
            return;

        switch (message[1])
        {
            case 'l':
                ship.TurnLeft();
                break;
            case 'r':
                ship.TurnRight();
                break;
            case 's':
                ship.Shoot();
                break;
            case 'e':
                ship.EnableEngine(message[2] == '1' ? true : false);
                break;
        }
    }

    private void ReadRequest(string message)
    {
        switch (message[1])
        {
            case 'n':
                name = message.Substring(2);
                playerName = message.Substring(2);
                LobbyManager.Instance.AddName(name);
                break;
        }
    }
}