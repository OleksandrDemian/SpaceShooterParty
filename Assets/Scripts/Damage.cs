using UnityEngine;

public class Damage
{
    private int damage;
    private ShipController parent;

    public Damage(int damage)
    {
        this.damage = damage;
    }

    public void SetParentShip(ShipController parent)
    {
        this.parent = parent;
    }

    public virtual void ApplyDamage(GameObject target)
    {
        IDamagable damagable = target.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damage, parent);
        }
    }
}