using UnityEngine;

public class Asteroid : MonoBehaviour, IDamagable, IPoolable {

    private Attribute health;

    private Vector2 direction;
    private int speed = 1;
    private Rigidbody2D rb;

    void Start () {
        health = new Attribute(AttributeType.HEALTH, 50);
        health.onValueChange = OnHealthChanged;

        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        speed = Random.Range(1, 5);
        direction.Normalize();
	}
	
	void FixedUpdate () {
        rb.MovePosition((Vector2)transform.position + (direction * Time.deltaTime * speed));
	}

    void Update() {
        GameManager.Instance.CheckPosition(transform);
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        Debug.Log("Collide with: " + collider.gameObject.name);
        IDamagable damagable = collider.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.Damage(10 + Random.Range(-5, 15));
    }

    public void OnHealthChanged(int value) {
        Debug.Log(name + ": vita -> " + value);
        if (value < 0) {
            Disable();
        }
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }

    public void Damage(int amount)
    {
        health.Value -= amount;
    }

    public GOType Type
    {
        get
        {
            return GOType.ASTEROID;
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
