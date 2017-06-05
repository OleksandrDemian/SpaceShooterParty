using UnityEngine;

public class LevelUpBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        PopUp.ShowText(target.transform.position, "Level UP!", 1, Color.white, (target.transform.position.y > 0 ? PopUpAnimation.DOWN : PopUpAnimation.UP));
        controller.GetPlayer().Write(Command.ADDPOINT);
    }
}