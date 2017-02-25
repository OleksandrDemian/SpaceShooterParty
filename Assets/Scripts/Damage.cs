using UnityEngine;

public class Damage
{
    private int damage;

    public Damage(int damage)
    {
        this.damage = damage;
    }

    public virtual void ApplyDamage(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.Damage(damage);
    }
}