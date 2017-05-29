using UnityEngine;

public class DisableControl : Damage
{
    private float time;

    public DisableControl(ShipController parent, float time) : base(parent)
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