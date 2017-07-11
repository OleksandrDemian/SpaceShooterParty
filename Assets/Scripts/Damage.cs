using UnityEngine;

public enum DamageEvents
{
    HIT,
    KILL,
    MISS
}

public class Damage
{
    private IDamageListener listener;

    public Damage(IDamageListener listener)
    {
        this.listener = listener;
    }

    public virtual void ApplyDamage(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(listener.GetDamage(), listener);
        }
    }

    public IDamageListener GetListener()
    {
        return listener;
    }
}