using UnityEngine;

public class FreezeEngine : Damage
{
    private float time;

    public FreezeEngine(int amount, float time) : base(amount)
    {
        this.time = time;
    }

    public override void ApplyDamage(GameObject target)
    {
        base.ApplyDamage(target);
        ShipController controller = target.GetComponent<ShipController>();

        if (controller == null)
            return;

        controller.DisableControll(time);
        //Freeze engine code here
    }
}