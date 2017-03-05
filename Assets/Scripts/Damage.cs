using UnityEngine;

public delegate void OnDead(GameObject go);

public class Damage
{
    private int damage;
    private OnDead onDead;

    public Damage(int damage)
    {
        this.damage = damage;
    }

    public void SetOnDeadCallback(OnDead onDead)
    {
        this.onDead = onDead;
    }

    public virtual void ApplyDamage(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damage, onDead);
        }
    }
}