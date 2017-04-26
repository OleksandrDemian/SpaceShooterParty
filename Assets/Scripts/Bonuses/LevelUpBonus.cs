using UnityEngine;

public class LevelUpBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;
        PopUp.ShowText(target.transform.position, "Level UP!");
        controller.GetPlayer().Write(Converter.toString(Request.ADDPOINT));
    }
}