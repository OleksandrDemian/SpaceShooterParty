using UnityEngine;

public class DestroyShield : Damage
{
    private int amount;

    public DestroyShield(int amount, int damage) : base(damage)
    {
        this.amount = amount;
    }

    public override void ApplyDamage(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller != null)
        {
            controller.GetAttribute(AttributeType.SHIELD).Value -= amount;
        }
        base.ApplyDamage(target);
    }
}