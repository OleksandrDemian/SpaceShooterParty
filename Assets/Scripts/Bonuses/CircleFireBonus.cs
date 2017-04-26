using UnityEngine;

public class CircleFireBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        Attribute damage = controller.GetAttribute(AttributeType.DAMAGE);
        CircleFire fire = new CircleFire(damage, 8, controller);

        fire.Trigger();
    }
}