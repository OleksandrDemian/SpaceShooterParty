using System.Collections.Generic;
using UnityEngine;

public class DestroyShieldsBonus : Bonus
{
    private bool itSelf = false;

    public DestroyShieldsBonus()
    {
        itSelf = false;
    }

    public DestroyShieldsBonus(bool itSelf)
    {
        this.itSelf = itSelf;
    }

    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        Vector3 position = target.transform.position;

        if (itSelf)
        {
            PopUp.ShowText(position, "Destroy shields", 1, Color.white, GetAnimation(position));

            controller.GetAttribute(AttributeType.SHIELD).Value = 0;
        }
        else
        {
            PopUp.ShowText(position, "Destroy enemies shields", 1, Color.white, GetAnimation(position));

            List<Player> players = GameManager.Instance.GetPlayers();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == controller.GetPlayer())
                    continue;

                players[i].SpaceShip.GetAttribute(AttributeType.SHIELD).Value = 0;
            }
        }
        
    }
}