using UnityEngine;

public interface IDamageListener
{
    void DamageListener(DamageEvents result, GameObject target);
    int GetDamage();
    GameObject GetGameObject { get; }
}