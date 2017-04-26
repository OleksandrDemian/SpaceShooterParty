﻿using UnityEngine;

public class Asteroid : MonoBehaviour, IPoolable, IDamagable
{
    private Attribute health;

    private Vector2 direction;
    private int speed = 1;
    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        Initialize();
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

    private void OnHealthChanged(int value, int oldValue) {
        if (value < 0) {
            Disable();
        }
    }

    /// <summary>
    /// This method is used to initialize the asteroid
    /// </summary>
    public void Initialize()
    {
        transform.position = new Vector3(50, 50, 0);

        direction = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();

        float size = 1 + Random.Range(-0.2f, 0.5f);
        transform.localScale = Vector3.one;
        transform.localScale *= size;

        health = new Attribute(AttributeType.HEALTH, (int)((70 + Random.Range(-10, 10)) * size));
        health.onValueChange = OnHealthChanged;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = GameManager.ImagePooler.GetAsteroidSkin();

        speed = Random.Range(1, 5);
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
    public void Damage(int amount, ShipController controller)
    {
        health.Value -= amount;
        /*PopUp popup = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP).GetComponent<PopUp>();
        popup.Initialize(transform.position, amount.ToString(), Color.white);*/
        PopUp.ShowText(transform.position, amount.ToString(), 0.5f);
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }
}
