using UnityEngine;

public class HealBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();

        if (controller == null)
            return;

        PopUp.ShowText(target.transform.position, "Heal", 0, Color.white, PopUpAnimation.RIGHT);
        controller.GetAttribute(AttributeType.HEALTH).ResetValue();
    }
}
