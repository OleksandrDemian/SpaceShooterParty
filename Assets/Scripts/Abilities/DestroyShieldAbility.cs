using System.Collections.Generic;
using UnityEngine;

class DestroyShieldAbility : Ability
{
    //AMOUNT is unused!
    //private int amount;
    private ShipController parent;

    public DestroyShieldAbility(int amount, ShipController parent)
    {
        //this.amount = amount;
        this.parent = parent;
    }

    public override void Trigger()
    {
        PopUp.ShowText(parent.transform.position, "Destroy shield");
        Laser controller = GameManager.ObjectPooler.Get<Laser>();
        if (controller != null)
        {
            controller.Initialize(parent.transform.position, parent.transform.rotation, new DestroyShield(parent));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(2));
        }
    }
}
