using System;
using UnityEngine;

class CircleFire : Ability
{
    private Attribute damage;
    private int qta = 3;
    private ShipController shipParent;

    public CircleFire(Attribute damage, int qta, ShipController parent) {
        if (damage.Type != AttributeType.DAMAGE)
            throw new ArgumentException("Attribute must be of type DAMAGE");

        this.damage = damage;
        this.qta = qta;
        shipParent = parent;
    }

    public override void Trigger()
    {
        PopUp.ShowText(shipParent.transform.position, "Circle fire");
        int delta = 360 / qta;
        int angle = (int)shipParent.transform.rotation.eulerAngles.z;
        for (int i = 0; i < qta; i++) {
            Shoot(angle);
            angle += delta;
        }
    }

    private void Shoot(int direction)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, direction);
        Laser bullet = GameManager.ObjectPooler.Get<Laser>();
        bullet.Initialize(shipParent.transform.position, rotation, new Damage(damage.Value));
        bullet.GetDamage().SetParentShip(shipParent);
    }
}