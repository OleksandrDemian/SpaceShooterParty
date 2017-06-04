using System;
using UnityEngine;

public delegate void OnConnectionClose(Player player);

public class Player : MonoBehaviour, IJoystickListener
{
    private ConnectionReader reader;
    private ShipController ship;
    private bool controllEnabled = false;

    private ShipInfo shipInfo;

    private Transform respawnPoint;

    //private string playerName = "Nameless";

    private PlayerStatistic statistics;

    public OnConnectionClose onConnectionClose;
    private float timeToDisconect = 2f;

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
        statistics = new PlayerStatistic();
    }

    private void Update()
    {
        timeToDisconect -= Time.deltaTime;
        if (timeToDisconect < 0)
        {
            CloseConnection();
            return;
        }
        reader.Read();

        //Controll ship here
        ControllShip();
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

    public void Write(Command command)
    {
        Write(Converter.ToChar(command).ToString());
    }

    public void InitializeShip(int shipNumber)
    {
        ship = GameManager.ObjectPooler.Get<ShipController>().GetComponent<ShipController>();
        ship.SetPlayer(this);

        if (shipInfo == null)
            return;

        shipInfo.InitializeShip(ship, shipNumber);
        ResetShipPosition();
        ship.SetRotationTarget((int)respawnPoint.rotation.eulerAngles.z);
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
                Write(Converter.ToChar(Command.HEALTH).ToString() + value);
                break;

            case AttributeType.SHIELD:
                Write(Converter.ToChar(Command.SHIELD).ToString() + value);
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
        timeToDisconect = 2;
        Debug.Log("Receive: " + message);

        switch (Converter.toCommand(message[0]))
        {
            case Command.COMMANDSSTRING:
                ReadCommand(message);
                break;

            case Command.NAME:
                if (LobbyManager.Instance == null)
                    return;
                string value = message.Substring(1);
                name = value;
                //playerName = value;
                LobbyManager.Instance.AddName(name);
                break;

            case Command.SHIPINFO:
                GetShipInfo(message.Substring(1));
                break;

            case Command.STARTGAME:
                if (LobbyManager.Instance == null)
                    return;
                LobbyManager.Instance.StartGame();
                break;

            case Command.PAUSE:
                PauseScreen.Instance.PauseTrigger();
                break;
        }
        
    }

    public void SetConnectionReader(ConnectionReader reader)
    {
        this.reader = reader;
        reader.SetJoystickListener(this);
        DontDestroyOnLoad(gameObject);
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
        Write(Command.KILL);
    }

    public void ShootResult(DamageEvents result)
    {
        Debug.Log("Reg: " + result);
        switch (result)
        {
            case DamageEvents.HIT:
                statistics.Hit();
                return;
            case DamageEvents.KILL:
                statistics.Kill();
                return;
            case DamageEvents.MISS:
                statistics.Miss();
                return;
        }
    }

    public PlayerStatistic GetStatistic()
    {
        return statistics;
    }

    public void Death()
    {
        statistics.Dead();

        Write(Command.DEAD );
        ship.gameObject.SetActive(false);
        EnableControll(false);
        GameTime.Instance.AddTimer(new Timer(3, delegate ()
        {
            Respawn();
        }));
    }

    public string Name
    {
        get { return name; }
    }

    public int ID
    {
        get { return reader.ID; }
    }

    public SocketInputManager Input
    {
        get { return input; }
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

    public void CloseConnection()
    {
        if (!enabled)
            return;

        enabled = false;
        reader.CloseConnection();
        Debug.Log("Player: " + name + " left the game");
        if (onConnectionClose != null)
            onConnectionClose(this);

        if(ship != null)
            ship.gameObject.SetActive(false);        
    }
}