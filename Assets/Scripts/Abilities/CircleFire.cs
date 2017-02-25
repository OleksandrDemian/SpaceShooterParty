using System;
using UnityEngine;

class CircleFire : Ability
{
    private Attribute damage;
    private int qta = 2;
    private Transform playerPosition;

    public CircleFire(Attribute damage, int qta, Transform position) {
        if (damage.Type != AttributeType.DAMAGE)
            throw new ArgumentException("Attribute must be of type DAMAGE");

        this.damage = damage;
        this.qta = qta;
        playerPosition = position;
    }

    public override void Trigger()
    {
        int delta = (int)(360 / qta);
        int angle = 0;
        for (int i = 0; i < qta; i++) {
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject laser = GameManager.ObjectPooler.Get(GOType.LASER) as GameObject;
            BulletController bullet = laser.GetComponent<BulletController>();
            bullet.Initialize(playerPosition.position, rotation, new Damage(damage.Value));
            angle += delta;
        }
    }
}