using UnityEngine;

public class LevelUpBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        PopUp.ShowText(target.transform.position, "Level UP!", 1, Color.white, GetAnimation(target.transform.position));
        controller.GetPlayer().Write(Command.ADDPOINT);
    }
}