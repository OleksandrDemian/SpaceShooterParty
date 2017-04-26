
using System;
using UnityEngine;

public class DoubleDamageBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;
        PopUp.ShowText(target.transform.position, "Double damage!");
        Attribute damage = controller.GetAttribute(AttributeType.DAMAGE);
        AttributeModifier mod = new AttributeModifier(ModifierType.MULTIPLY, 2);
        damage.AddModifier(mod);
        GameTime.Instance.AddTimer(new Timer(10, delegate() {
            PopUp.ShowText(target.transform.position, "Double damage removed!");
            damage.RemoveModifier(mod);
        }));
    }
}