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
        PopUp.ShowText(transform.position, "Destroy shield");
        Laser controller = GameManager.ObjectPooler.Get<Laser>();
        if (controller != null)
        {
            controller.Initialize(transform.position, transform.rotation, new DestroyShield(3));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(2));
        }
    }
}
