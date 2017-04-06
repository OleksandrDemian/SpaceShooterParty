using UnityEngine;

public class Asteroid : MonoBehaviour, IPoolable, IDamagable
{
    private Attribute health;

    private Vector2 direction;
    private int speed = 1;
    private Rigidbody2D rb;

    void Start () {
        health = new Attribute(AttributeType.HEALTH, 50 + Random.Range(-10, 10));
        health.onValueChange = OnHealthChanged;

        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = GameManager.ImagePooler.GetAsteroidSkin();

        speed = Random.Range(1, 5);
        direction.Normalize();
	}
	
	void FixedUpdate () {
        rb.MovePosition((Vector2)transform.position + (direction * GameTime.TimeScale * speed));
	}

    void Update() {
        GameManager.Instance.CheckPosition(transform);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.Damage(10 + Random.Range(-5, 15), null);
    }

    public void OnHealthChanged(int value) {
        if (value < 0) {
            Disable();
        }
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }

    public void Damage(int amount, OnDead onDead)
    {
        health.Value -= amount;
        PopUp popup = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP).GetComponent<PopUp>();
        popup.Initialize(transform.position, amount.ToString(), Color.white);
        if(onDead != null && health.Value < 0)
            onDead(gameObject);
    }

    public EntityType Type
    {
        get
        {
            return EntityType.ASTEROID;
        }
    }

    public GameObject Get
    {
        get
        {
            return gameObject;
        }
    }
}
