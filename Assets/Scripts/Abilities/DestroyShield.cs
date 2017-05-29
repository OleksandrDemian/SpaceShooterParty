using UnityEngine;

public class DestroyShield : Damage
{
    //private int amount;

    public DestroyShield(ShipController parent) : base(parent)
    {
        //this.amount = amount;
    }

    public override void ApplyDamage(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller != null)
        {
            controller.GetAttribute(AttributeType.SHIELD).Value = 0;
            //PopUp.ShowText(target.transform.position, "Shield destroyed", 0.5f, Color.red);
        }
        base.ApplyDamage(target);
    }
}