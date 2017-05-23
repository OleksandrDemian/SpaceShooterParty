using UnityEngine;

public class HealBonus : Bonus
{
    private int amount = 0;

    public HealBonus()
    {
        amount = 0;
    }

    public HealBonus(int amount)
    {
        this.amount = amount;
    }

    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();

        if (controller == null)
            return;

        if (amount == 0)
        {
            controller.GetAttribute(AttributeType.HEALTH).ResetValue();
        }
        else
        {
            controller.GetAttribute(AttributeType.HEALTH).Value += amount;
        }
        if(amount >= 0)
            PopUp.ShowText(target.transform.position, "Heal", 1, Color.white, PopUpAnimation.RIGHT);
        else
            PopUp.ShowText(target.transform.position, "Damage", 1, Color.white, PopUpAnimation.RIGHT);
    }
}
