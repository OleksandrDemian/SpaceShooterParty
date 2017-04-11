﻿using UnityEngine;

public class DisableControllAbility : Ability
{
    private float time;
    private Transform transform;

    public DisableControllAbility(float time, Transform transform)
    {
        this.time = time;
        this.transform = transform;
    }

    public override void Trigger()
    {
        PopUp.ShowText(transform.position, "Disable controll");
        GameObject laser = GameManager.ObjectPooler.Get(EntityType.LASER) as GameObject;
        BulletController controller = laser.GetComponent<BulletController>();
        if (controller != null) {
            controller.Initialize(transform.position, transform.rotation, new DisableControll(2, time));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(1));
        }
    }
}