using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour, IPoolable {

    private int damage;
    private Rigidbody2D rb;
    private float timer = 0f;
    private int speed = 10;

    private void Start () {
        rb = GetComponent<Rigidbody2D>();
        Get = this.gameObject;
        Type = GOType.LASER;
	}

    private void Update() {
        timer += Time.deltaTime;
        if (timer > 3) {
            Disable();
        }
        GameManager.Instance.CheckPosition(transform);
    }

	private void FixedUpdate () {
        rb.MovePosition(transform.position + transform.up * Time.deltaTime * speed);
	}

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Colpito");
        IDamagable damagable = collider.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.Damage(damage);
            Disable();
        }
    }

    private void Disable() {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }

    public void Initialize(Transform transform, int damage) {
        this.damage = damage;
        this.transform.rotation = transform.rotation;
        this.transform.position = transform.position + transform.up;
        timer = 0f;
    }

    public GOType Type
    {
        get;
        private set;
    }

    public GameObject Get
    {
        get;
        private set;
    }
}
