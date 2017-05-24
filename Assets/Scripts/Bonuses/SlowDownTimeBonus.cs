using UnityEngine;

class SlowDownTimeBonus : Bonus
{
    private float target = 0.2f;

    public SlowDownTimeBonus()
    {
        target = 0.2f;
    }

    public SlowDownTimeBonus(float target)
    {
        this.target = target;
    }

    public override void Trigger(GameObject target)
    {
        if(this.target < 1)
            PopUp.ShowText(target.transform.position, "Slow down time!", 1);
        else
            PopUp.ShowText(target.transform.position, "Speed up time!", 1);

        GameTime gt = GameTime.Instance;

        gt.SetTimeScaleTarget(this.target);
        gt.AddTimer(new Timer(6 * this.target, delegate() {
            gt.SetTimeScaleTarget(1);
        }));
    }
}