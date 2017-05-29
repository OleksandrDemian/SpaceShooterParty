using UnityEngine;

public class CircleFireBonus : Bonus
{
    private int lasersCount = 8;

    public CircleFireBonus()
    {
        lasersCount = 8;
    }

    public CircleFireBonus(int lasersCount)
    {
        this.lasersCount = lasersCount;
    }

    public override void Trigger(GameObject target)
    {
        ShipController controller = target.GetComponent<ShipController>();
        if (controller == null)
            return;

        CircleFire fire = new CircleFire(lasersCount, controller);

        fire.Trigger();
    }
}