using UnityEngine;

public class MultiplyDamageBonus : Bonus
{
    private int mult = 2;

    public MultiplyDamageBonus()
    {
        mult = 2;
    }

    public MultiplyDamageBonus(int mult)
    {
        this.mult = mult;
    }

    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        Vector3 position = target.transform.position;
        PopUp.ShowText(position, "Damage x" + mult, 1, Color.white, (position.y > 0 ? PopUpAnimation.DOWN : PopUpAnimation.UP));

        Attribute damage = controller.GetAttribute(AttributeType.DAMAGE);
        AttributeModifier mod = new AttributeModifier(ModifierType.MULTIPLY, mult);
        damage.AddModifier(mod);
        GameTime.Instance.AddTimer(new Timer(10, delegate() {
            PopUp.ShowText(target.transform.position, "Multiply damage removed!", 1);
            damage.RemoveModifier(mod);
        }));
    }
}