public abstract class Ability
{
    public abstract void Trigger();

    public virtual int RechargeTime
    {
        get
        {
            return 3;
        }
    }
}