using UnityEngine;

public class Enemy : MonoBehaviour {

    protected int speed = 5;
    protected Vector2 targetPosition = Vector2.zero;
    protected float fireRate = 2;

	protected void Start ()
    {
        targetPosition = GetRandomPosition();
        GameTime.Instance.AddTimer(new Timer(fireRate + Random.Range(0f, 2f), delegate ()
        {
            Fire();
        }));
	}
	
	protected void Update ()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * GameTime.TimeScale);
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            targetPosition = GetRandomPosition();
        }
	}

    protected virtual void Fire()
    {
        Laser laser = ObjectPool.Instance.Get<Laser>();
        if (laser == null)
            return;

        laser.Initialize(transform.position + transform.forward, transform.rotation, null);

        GameTime.Instance.AddTimer(new Timer(fireRate + Random.Range(0f, .5f), delegate ()
        {
            Fire();
        }));
    }

    protected static Vector2 GetRandomPosition()
    {
        Vector2 bounds = GameManager.Instance.MapBounds;
        float x = Random.Range(-bounds.x + 1, bounds.x - 1);
        float y = Random.Range(0, bounds.y - 1);

        return new Vector2(x, y);
    }
}
