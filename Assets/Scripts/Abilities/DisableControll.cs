using UnityEngine;

public class DisableControll : Damage
{
    private float time;

    public DisableControll(int amount, float time) : base(amount)
    {
        this.time = time;
    }

    public override void ApplyDamage(GameObject target)
    {
        base.ApplyDamage(target);
        ShipController controller = target.GetComponent<ShipController>();

        if (controller == null)
            return;

        controller.GetPlayer().EnableControll(false);

        Timer timer = new Timer(time, delegate ()
        {
            controller.GetPlayer().EnableControll(true);
        });

        GameTime.Instance.AddTimer(timer);
    }
}