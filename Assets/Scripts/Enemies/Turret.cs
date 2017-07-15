using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IDamageListener, IPoolable, IDamagable, IBlackHoleAttractable
{
    private Transform target;
    private Fire fire;
    private int damage = 10;
    private Transform head;
    private Attribute health;

    public GameObject GetGameObject
    {
        get
        {
            return head.gameObject;
        }
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        head = transform.FindChild("Head");

        fire = new Fire(this, 2);
        fire.SetMode(FireMode.SERIALFIRE);

        health = new Attribute(AttributeType.HEALTH, 50 + Random.Range(-10, 20));
        health.onValueChange = OnHealthChanged;

        damage = Random.Range(8, 16);

        GetTarget(GameManager.Instance.GetPlayers());
    }

    private void OnHealthChanged(int value, int oldValue)
    {
        if (value < 1)
        {
            ExplosionManager expManager = ObjectPool.Instance.Get<ExplosionManager>();
            expManager.Initialize(transform.position, 7);
            Disable();
        }
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }

    private void Update ()
    {
        LookAtTarget();
    }

    private void LookAtTarget()
    {
        if (target == null)
        {
            head.transform.Rotate(0, 0, GameTime.TimeScale * 10);
            return;
        }

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        float z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        head.rotation = Quaternion.Euler(0, 0, z - 90);
        fire.Trigger();
    }

    private void GetTarget(List<Player> players)
    {
        float distance = Mathf.Infinity;

        for (int i = 0; i < players.Count; i++)
        {
            float curDistance = Vector3.Distance(transform.position, players[i].SpaceShip.transform.position);
            if (curDistance < distance)
            {
                distance = curDistance;
                target = players[i].SpaceShip.transform;
            }
        }
    }

    public void DamageListener(DamageEvents result, GameObject target)
    {
        if (result == DamageEvents.KILL)
        {
            GetTarget(GameManager.Instance.GetPlayers());
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Damage(int amount, IDamageListener resultListener)
    {
        health.Value -= amount;

        PopUp.ShowText(transform.position, amount.ToString(), 0.5f, Color.white, PopUpAnimation.GRAVITY);

        if (resultListener == null)
            return;

        if (health.Value > 0)
            resultListener.DamageListener(DamageEvents.HIT, gameObject);
        else if(health.Value < 1)
            resultListener.DamageListener(DamageEvents.KILL, gameObject);        
    }

    public void Attract(Vector3 toPosition)
    {
        if (!gameObject.activeInHierarchy)
            return;

        float distance = Vector3.Distance(transform.position, toPosition);
        if (distance < 0.5f)
        {
            health.Value = 0;
        }
        else
        {
            Vector3 direction = toPosition - transform.position;
            transform.Translate(50 * GameTime.TimeScale * direction.normalized / distance);
        }
    }
}
