using UnityEngine;

class CircleFire : Ability
{
    private int qta = 3;
    private ShipController shipParent;

    public CircleFire(int qta, ShipController parent) {
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
        bullet.Initialize(shipParent.transform.position, rotation, new Damage(shipParent));
        bullet.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
        //bullet.GetDamage().SetDamageListener(shipParent.DamageListener);
    }

    public override int RechargeTime
    {
        get
        {
            return base.RechargeTime;
        }
    }
}