using UnityEngine;

public class KeyBoardPlayer : MonoBehaviour {

    private ShipController ship;

	void Start ()
    {
        ship = FindObjectOfType<ShipController>();
        LoadDefaultShip();
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ship.Fire();
        }
        if (Input.GetKey(KeyCode.A))
        {
            ship.TurnLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            ship.TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ship.EngineTrigger();
        }
    }

    private void LoadDefaultShip()
    {
        ship.SetDamage(10);
        ship.SetHealth(20);
        ship.SetShield(3);
        ship.SetSpeed(600);
    }
}
