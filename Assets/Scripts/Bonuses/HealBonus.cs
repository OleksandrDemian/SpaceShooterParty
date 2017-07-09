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
        Vector3 position = target.transform.position;
        if(amount >= 0)
            PopUp.ShowText(position, "Heal", 1, Color.white, GetAnimation(position));
        else
            PopUp.ShowText(position, "Damage", 1, Color.red, GetAnimation(position));
    }
}
