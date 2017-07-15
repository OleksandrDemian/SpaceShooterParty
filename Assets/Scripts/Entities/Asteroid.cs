using UnityEngine;

public class Asteroid : MonoBehaviour, IPoolable, IDamagable, IBlackHoleAttractable
{
    private Attribute health;

    private Vector2 direction;
    private int speed = 1;
    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
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
            damagable.Damage((int)(transform.localScale.x * 10), null);
    }

    private void OnHealthChanged(int value, int oldValue) {
        if (value < 1) {
            ExplosionManager expManager = ObjectPool.Instance.Get<ExplosionManager>();
            expManager.Initialize(transform.position, 6);
                        
            if (transform.localScale.x > 1)
            {
                int qta = Random.Range(0, 4);
                for (int i = 0; i < qta; i++)
                {
                    Asteroid ast = ObjectPool.Instance.Get<Asteroid>();
                    ast.Initialize(transform.position);
                }
            }
            

            Disable();
        }
    }

    /// <summary>
    /// This method is used to initialize the asteroid
    /// </summary>
    public void Initialize()
    {
        transform.position = new Vector3(GameManager.Instance.MapBounds.x, GameManager.Instance.MapBounds.y, 0);

        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();

        float size = Random.Range(.9f, 1.5f);

        transform.localScale = Vector3.one * size;

        health = new Attribute(AttributeType.HEALTH, (int)((70 + Random.Range(-10, 10)) * size));
        health.onValueChange = OnHealthChanged;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = GameManager.ImagePooler.GetAsteroidSkin();

        speed = Random.Range(1, 5);
    }

    
    public void Initialize(Vector3 position)
    {
        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();

        transform.position = position + (Vector3)direction;

        float size = Random.Range(.7f, .9f);

        transform.localScale = Vector3.one * size;

        health = new Attribute(AttributeType.HEALTH, (int)((50 + Random.Range(-10, 10)) * size));
        health.onValueChange = OnHealthChanged;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = GameManager.ImagePooler.GetAsteroidSkin();

        speed = Random.Range(3, 6);
    }
    
    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// It is used for damaging the asteroid
    /// </summary>
    /// <param name="amount">amount of damage</param>
    /// <param name="onDead">a function called when the asteroid is destroed</param>
    public void Damage(int amount, IDamageListener listener)
    {
        health.Value -= amount;

        if(listener != null)
            listener.DamageListener(DamageEvents.HIT, gameObject);
        /*PopUp popup = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP).GetComponent<PopUp>();
        popup.Initialize(transform.position, amount.ToString(), Color.white);*/
        PopUp.ShowText(transform.position, amount.ToString(), 0.5f, Color.white, PopUpAnimation.GRAVITY);
    }

    public void Attract(Vector3 toPosition)
    {
        Vector3 direction = toPosition - transform.position;
        float distance = Vector3.Distance(transform.position, toPosition);
        this.direction = Vector2.Lerp(this.direction, direction.normalized / distance * 25, GameTime.TimeScale);
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }
}
