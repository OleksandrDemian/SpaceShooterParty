using System;
using System.Collections;
using UnityEngine;

public delegate void OnConnectionClose(Player player);

public class Player : MonoBehaviour, IJoystickListener
{
    private ConnectionReader reader;
    private ShipController ship;
    private bool controllEnabled = false;

    private ShipInfo shipInfo;

    private Transform respawnPoint;

    private string playerName = "Commander";
    public int kill = 0;
    public int dead = 0;
    public OnConnectionClose onConnectionClose;

    public ShipController SpaceShip
    {
        get
        {
            return ship;
        }
    }

    public ConnectionReader Connection
    {
        get
        {
            return reader;
        }
    }

    private void Start()
    {
        
    }

    private float time = 0f;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            Write("p");
            time = 0;
        }
        reader.Read();
    }

    public void Write(string message) {
        try
        {
            reader.Write(message);
        }
        catch
        {
            Debug.Log("Player: " + playerName + " left the game");
            CloseConnection();
        }
    }

    public void InitializeShip(int shipNumber)
    {
        ship = GameManager.ObjectPooler.Get(EntityType.SHIP).GetComponent<ShipController>();
        ship.SetPlayer(this);

        if (shipInfo == null)
            return;

        Debug.Log("Ship: " + shipInfo.ToString());
        shipInfo.InitializeShip(ship, shipNumber);
        ResetShipPosition();
        onConnectionClose = null;
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void EnableControll(bool action)
    {
        controllEnabled = action;
    }

    public void OnMessageRead(string message)
    {
        //Debug.Log("Message: " + message);
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
        EnableControll(false);
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3);
        ResetShipPosition();
        ship.gameObject.SetActive(true);
        ship.ResetAttributes();
        EnableControll(true);
    }

    public void ResetShipPosition()
    {
        ship.transform.position = respawnPoint.position;
        ship.transform.rotation = respawnPoint.rotation;
    }

    public void Kill(GameObject killed)
    {
        Debug.Log(Name + " killed " + killed.name);
        kill++;
        //reader.Write("Kill");
    }

    public string Name
    {
        get { return playerName; }
    }

    public int ID
    {
        get { return reader.ID; }
    }

    public void SendRequest(Request request)
    {
        Write(Converter.toString(request));
    }

    private void ReadCommand(string message)
    {
        if (!controllEnabled)
            return;

        switch (Converter.toCommand(message[1]))
        {
            case Command.TURNLEFT:
                ship.TurnLeft();
                break;
            case Command.TURNRIGHT:
                ship.TurnRight();
                break;
            case Command.FIRE:
                ship.Fire();
                break;
            case Command.ENGINETRIGGER:
                ship.EngineTrigger();
                break;
            case Command.ABILITYTRIGGER:
                ship.AbilityTrigger();
                break;
        }
    }

    private void ReadRequest(string message)
    {
        switch (Converter.toRequest(message[1]))
        {
            case Request.NAME:
                if (LobbyManager.Instance == null)
                    return;
                name = message.Substring(2);
                playerName = message.Substring(2);
                LobbyManager.Instance.AddName(name);
                break;
            case Request.SHIPINFO:
                GetShipInfo(message.Substring(2));
                break;
        }
    }

    private void GetShipInfo(string message)
    {
        Debug.Log("ShipInfo: " + message);
        try
        {
            shipInfo = new ShipInfo();
            string[] parts = message.Split(':');

            shipInfo.health = int.Parse(parts[1]);
            shipInfo.damage = int.Parse(parts[2]);
            shipInfo.shield = int.Parse(parts[3]);
            shipInfo.speed = int.Parse(parts[4]);
            shipInfo.shipSkin = int.Parse(parts[5]);
            shipInfo.abilityType = (AbilityType)int.Parse(parts[6]);
            shipInfo.abilityLevel = int.Parse(parts[7]);

            Debug.Log(shipInfo.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Ship info generation failed");
            Debug.Log(e.Message);
        }
    }

    private void CloseConnection()
    {
        if(onConnectionClose != null)
            onConnectionClose(this);

        if(ship != null)
            ship.gameObject.SetActive(false);        
        enabled = false;
    }
}