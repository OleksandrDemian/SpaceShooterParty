using UnityEngine;
using System.Collections.Generic;

public class PowerUp : MonoBehaviour, IPoolable
{
    private List<Bonus> bonuses = new List<Bonus>();

    private Transform target;
    private bool followTarget = false;
    private float lastTargetCheck = 0;

    private void Update()
    {
        if (followTarget)
            FollowTarget();
    }

    private void FollowTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, GameTime.TimeScale * 6);
        if (lastTargetCheck > GameTime.GetTime() + 2)
        {
            lastTargetCheck = GameTime.GetTime();
            GetTarget();
        }
    }

    public void EnableFollowing(bool action)
    {
        followTarget = action;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (action)
        {
            GetTarget();
            sprite.sprite = GameManager.ImagePooler.GetPowerUpSkin(1);
        }
        else
        {
            sprite.sprite = GameManager.ImagePooler.GetPowerUpSkin(0);
        }
    }

    private void GetTarget()
    {
        List<Player> players = GameManager.Instance.GetPlayers();

        float distance = Mathf.Infinity;

        for (int i = 0; i < players.Count; i++)
        {
            float distanceTemp = Vector2.Distance(transform.position, players[i].SpaceShip.transform.position);
            if (distanceTemp > distance)
                continue;

            target = players[i].SpaceShip.transform;
            distance = distanceTemp;
        }
    }

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