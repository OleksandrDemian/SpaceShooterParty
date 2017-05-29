using UnityEngine;

public delegate void OnDamage(DamageEvents result, GameObject target);

public enum DamageEvents
{
    HIT,
    KILL,
    MISS
}

public class Damage
{
    private int damage;
    private OnDamage onDamage;

    public Damage(int damage)
    {
        this.damage = damage;
    }

    public void SetDamageListener(OnDamage listener)
    {
        onDamage = listener;
    }

    public virtual void ApplyDamage(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damage, onDamage);
        }
    }

    public OnDamage GetOnDamage()
    {
        return onDamage;
    }
}