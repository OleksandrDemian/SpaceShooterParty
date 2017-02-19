using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour, IPoolable {

    private int damage;
    private Rigidbody2D rb;
    private float timer = 0f;
    private int speed = 10;

    private void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Disable();
        }
        GameManager.Instance.CheckPosition(transform);
    }

	private void FixedUpdate ()
    {
        rb.MovePosition(transform.position + transform.up * Time.deltaTime * speed);
	}

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Colpito");
        IDamagable damagable = collider.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damage);
            Disable();
        }
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }

    public void Initialize(Vector3 position, Quaternion rotation, int damage)
    {
        this.damage = damage;
        transform.rotation = rotation;
        transform.position = position + transform.up;
        timer = 0f;
    }

    public GOType Type
    {
        get
        {
            return GOType.LASER;
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
