using UnityEngine;

public class Turret : MonoBehaviour, IDamageListener
{
    private Transform target;
    private Fire fire;
    private int damage;

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }

    private void Start ()
    {
        fire = new Fire(this, 1);
        GetTarget();
	}
	
	private void Update ()
    {
		
	}

    private void GetTarget()
    {
        Player[] players = GameManager.Instance.GetPlayers().ToArray();

        float distance = Mathf.Infinity;
        int index = 0;

        for (int i = 0; i < players.Length; i++)
        {
            float curDistance = Vector3.Distance(transform.position, players[i].SpaceShip.transform.position);
            if (curDistance < distance)
            {
                distance = curDistance;
                index = i;
            }
        }

        target = players[index].SpaceShip.transform;
    }

    public void DamageListener(DamageEvents result, GameObject target)
    {
        
    }

    public int GetDamage()
    {
        return damage;
    }
}
