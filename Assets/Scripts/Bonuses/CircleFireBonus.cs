using UnityEngine;

public class CircleFireBonus : Bonus
{
    private int lasersCount = 8;

    public CircleFireBonus()
    {
        lasersCount = 8;
    }

    public CircleFireBonus(int lasersCount)
    {
        this.lasersCount = lasersCount;
    }

    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        Attribute damage = controller.GetAttribute(AttributeType.DAMAGE);
        CircleFire fire = new CircleFire(damage, lasersCount, controller);

        fire.Trigger();
    }
}