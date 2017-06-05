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

        if (itSelf)
        {
            PopUp.ShowText(target.transform.position, "Destroy shields", 1);

            controller.GetAttribute(AttributeType.SHIELD).Value = 0;
        }
        else
        {
            Vector3 position = target.transform.position;
            PopUp.ShowText(position, "Destroy enemies shields", 1, Color.white, (position.y > 0 ? PopUpAnimation.DOWN : PopUpAnimation.UP));

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