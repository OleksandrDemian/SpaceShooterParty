using UnityEngine;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour, IPoolable
{
    private List<Bonus> bonuses = new List<Bonus>();

    public void AddBonus(Bonus bonus)
    {
        bonuses.Add(bonus);
    }

    public void ClearBonuses()
    {
        bonuses.Clear();
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;

        for (int i = 0; i < bonuses.Count; i++)
            bonuses[i].Trigger(other.gameObject);

        Disable();
    }

    private void Disable()
    {
        ClearBonuses();
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }
}