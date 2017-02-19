using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IJoystickListener
{
    private ConnectionReader reader;
    private ShipController ship;
    private ShipInfo shipInfo;
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

    public void InitializeShip(int shipNumber)
    {
        ship = GameManager.ObjectPooler.Get(GOType.SHIP).GetComponent<ShipController>();
        ship.SetPlayer(this);

        if (shipInfo == null)
            return;

        Debug.Log("Ship: " + shipInfo.ToString());
        ship.SetHealth(shipInfo.health);
        ship.SetDamage(shipInfo.damage);
        ship.SetSpeed(shipInfo.speed);
        ship.SetShield(shipInfo.shield);
        ship.SetImage(GameManager.ObjectPooler.GetShipSkin(shipInfo.shipSkin * shipNumber));
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

    public ShipController SpaceShip {
        get
        {
            return ship;
        }
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

            Debug.Log(shipInfo.ToString());
        }
        catch (Exception e)
        {
            Debug.Log("Ship info generation failed");
            Debug.Log(e.Message);
        }
        
    }
}