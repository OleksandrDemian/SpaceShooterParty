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
        Vector3 position = target.transform.position;
        string shownText = "Slow down time!";
        if (this.target > 1)
            shownText = "Speed up time!";

        PopUp.ShowText(position, shownText, 1, Color.white, GetAnimation(position));

        GameTime gt = GameTime.Instance;

        gt.SetTimeScaleTarget(this.target);
        gt.AddTimer(new Timer(6 * this.target, delegate() {
            gt.SetTimeScaleTarget(1);
        }));
    }
}