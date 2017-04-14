using System.Collections.Generic;
using UnityEngine;

class DestroyShieldAbility : Ability
{
    //AMOUNT is unused!
    //private int amount;
    private Transform transform;

    public DestroyShieldAbility(int amount, Transform transform)
    {
        //this.amount = amount;
        this.transform = transform;
    }

    public override void Trigger()
    {
        /*GameObject laser = GameManager.ObjectPooler.Get(EntityType.LASER) as GameObject;
        BulletController controller = laser.GetComponent<BulletController>();
        if (controller != null)
        {
            controller.Initialize(transform.position, transform.rotation, new DestroyShield(5));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(2));
        }*/

        PopUp.ShowText(transform.position, "Destroy shield");
        //GameObject laser = GameManager.ObjectPooler.Get(EntityType.LASER) as GameObject;
        //GameObject laser = GameManager.ObjectPooler.Get<Laser>() as GameObject;
        Laser controller = GameManager.ObjectPooler.Get<Laser>();
        if (controller != null)
        {
            controller.Initialize(transform.position, transform.rotation, new DestroyShield(3));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(2));
        }
        //Player player = transform.GetComponent<ShipController>().GetPlayer();
        //List<Player> players = GameManager.Instance.GetPlayers();
        /*for (int i = 0; i < players.Count; i++)
        {
            if (player == players[i])
                continue;
            players[i].SpaceShip.GetAttribute(AttributeType.SHIELD).Value -= 3;
        }*/
    }
}
