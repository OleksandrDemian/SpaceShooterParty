using UnityEngine;

class SlowDownTimeBonus : Bonus
{
    public override void Trigger(GameObject target)
    {
        PopUp.ShowText(target.transform.position, "Slow down time!");

        GameTime gt = GameTime.Instance;

        gt.SetTimeScaleTarget(0.2f);
        gt.AddTimer(new Timer(3, delegate() {
            gt.SetTimeScaleTarget(1);
        }));
    }
}