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

    private string playerName = "Nameless";
    public int kill = 0;
    public int dead = 0;
    public OnConnectionClose onConnectionClose;

    //TEMP
    private SocketInputManager input = new SocketInputManager();

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

    //TEMP
    private float time = 0f;
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 2)
        {
            CloseConnection();
            return;
        }
        reader.Read();

        //Controll ship here
        ControllShip();
        //delta += Time.deltaTime;
    }

    //<------------------------------------------->
    private void ControllShip()
    {
        if (input.GetCommand(Command.FIRE))
        {
            ship.Fire();
        }

        if (input.GetCommand(Command.TURNRIGHT))
        {
            ship.TurnRight();
        }

        if (input.GetCommand(Command.TURNLEFT))
        {
            ship.TurnLeft();
        }

        if (input.GetCommand(Command.ENGINETRIGGER))
        {
            ship.EngineTrigger();
            input.RemoveCommand(Command.ENGINETRIGGER);
        }

        if (input.GetCommand(Command.ABILITYTRIGGER))
        {
            ship.AbilityTrigger();
            input.RemoveCommand(Command.ABILITYTRIGGER);
        }
    }
    //<------------------------------------------->

    public void Write(string message) {
        try
        {
            reader.Write(message);
        }
        catch
        {
            CloseConnection();
        }
    }

    public void InitializeShip(int shipNumber)
    {
        ship = GameManager.ObjectPooler.Get<ShipController>().GetComponent<ShipController>();
        ship.SetPlayer(this);

        if (shipInfo == null)
            return;

        Debug.Log("Ship: " + shipInfo.ToString());
        shipInfo.InitializeShip(ship, shipNumber);
        ResetShipPosition();
        //onConnectionClose = null;
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }

    public void OnShipValueChange(AttributeType type, int value)
    {
        switch (type)
        {
            case AttributeType.HEALTH:
                Write("ih" + value);
                break;

            case AttributeType.SHIELD:
                Write("is" + value);
                break;
        }
    }

    public void EnableControll(bool action)
    {
        controllEnabled = action;

        if (!controllEnabled)
            input.ClearCommands();
    }

    public void OnMessageRead(string message)
    {
        time = 0;
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
        //StartCoroutine(Respawn());
        GameTime.Instance.AddTimer(new Timer(3, delegate()
        {
            Respawn();
        }));
    }

    private void Respawn()
    {
        //yield return new WaitForSeconds(3);
        ResetShipPosition();
        ship.gameObject.SetActive(true);
        ship.ResetAttributes();
        EnableControll(true);
        ship.EnableCollider(false);
        GameTime.Instance.AddTimer(new Timer(3, delegate()
        {
            ship.EnableCollider(true);
        }));
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
    }

    public string Name
    {
        get { return playerName; }
    }

    public int ID
    {
        get { return reader.ID; }
    }

    public SocketInputManager Input
    {
        get { return input; }
    }

    public void SendRequest(Request request)
    {
        Write(Converter.toString(request));
    }

    //TEMP
    //float delta = 0;
    private void ReadCommand(string message)
    {
        if (!controllEnabled)
            return;

        input.ManageInput(message.Substring(1));
        //Debug.Log("Delta: " + delta);
        //delta = 0;
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
            case Request.STARTGAME:
                if (LobbyManager.Instance == null)
                    return;
                LobbyManager.Instance.StartGame();
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
            Debug.Log("Ship generation failed");
            Debug.Log(e.Message);
        }
    }

    private void CloseConnection()
    {
        if (!enabled)
            return;

        enabled = false;
        Debug.Log("Player: " + playerName + " left the game");
        if (onConnectionClose != null)
            onConnectionClose(this);

        if(ship != null)
            ship.gameObject.SetActive(false);        
    }
}