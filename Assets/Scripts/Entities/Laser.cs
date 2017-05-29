using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Laser : MonoBehaviour, IPoolable, IBlackHoleAttractable
{
    private Damage damage;
    private Rigidbody2D rb;
    private float timer = 0f;
    private int speed = 15;

    private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        timer += GameTime.TimeScale;
        if (timer > 2)
        {
            damage.GetOnDamage().Invoke(DamageEvents.MISS, null);
            Disable();
        }
        GameManager.Instance.CheckPosition(transform);
    }

	private void FixedUpdate ()
    {
        rb.MovePosition(transform.position + transform.up * GameTime.TimeScale * speed);
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "IgnoreLasers")
            return;

        damage.ApplyDamage(collider.gameObject);
        LaserTrigger effect = GameManager.ObjectPooler.Get<LaserTrigger>();
        effect.Initialize(transform.position);
        Disable();
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }

    public void Initialize(Vector3 position, Quaternion rotation, Damage damage)
    {
        this.damage = damage;
        transform.rotation = rotation;
        transform.position = position + transform.up;
        timer = 0f;
    }

    public Damage GetDamage()
    {
        return damage;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void Attract(Vector3 toPosition)
    {
        //return;

        Vector3 direction = toPosition - transform.position;
        float distance = Vector3.Distance(transform.position, toPosition);

        transform.up = Vector3.Slerp(transform.up, direction, (GameTime.TimeScale * 2) / distance);
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }
}
