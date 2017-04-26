using System.Collections.Generic;
using UnityEngine;

public class DestroyShieldsBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        PopUp.ShowText(target.transform.position, "Destroy enemies shields");
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        List<Player> players = GameManager.Instance.GetPlayers();
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == controller.GetPlayer())
                continue;

            players[i].SpaceShip.GetAttribute(AttributeType.SHIELD).Value = 0;
        }
    }
}