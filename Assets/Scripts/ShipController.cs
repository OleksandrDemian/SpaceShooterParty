using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour, IDamagable {

    private Attribute speed;
    private Attribute rotationSpeed;
    private Attribute health;
    private Attribute damage;
    private Attribute shield;

    private int rotationTarget = 0;
    private Rigidbody2D rb;
    private Player player;
    private SpriteRenderer engineTrail;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        engineTrail = transform.Find("Trail").GetComponent<SpriteRenderer>();

        speed = new Attribute(AttributeType.SPEED, 500);
        rotationSpeed = new Attribute(AttributeType.ROTATIONSPEED, 10);
        damage = new Attribute(AttributeType.DAMAGE, 15);
        health = new Attribute(AttributeType.HEALTH, 50);
        health.onValueChange = OnHealthValueChange;
        shield = new Attribute(AttributeType.SHIELD, 15);

        EnableEngine(false);
    }

    void Update()
    {
       /* if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
        if (Input.GetKeyDown(KeyCode.A))
            TurnLeft();
        if (Input.GetKeyDown(KeyCode.D))
            TurnRight();
        if (Input.GetKeyDown(KeyCode.W))
            EnableEngine(true);
        if (Input.GetKeyDown(KeyCode.S))
            EnableEngine(false);*/
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationTarget), rotationSpeed.Value * Time.deltaTime);
        GameManager.Instance.CheckPosition(transform);
    }

	void FixedUpdate () {
        rb.AddForce(transform.up * speed.Value * Time.deltaTime, ForceMode2D.Force);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void Shoot()
    {
        GameObject bullet = GameManager.ObjectPooler.Get(GOType.LASER);
        BulletController controller = bullet.GetComponent<BulletController>();
        controller.Initialize(transform, damage.Value);
    }

    public void TurnRight()
    {
        rotationTarget -= 30;
    }

    public void TurnLeft()
    {
        rotationTarget += 30;
    }

    public void EnableEngine(bool action)
    {
        engineTrail.enabled = action;
        if (action)
            speed.ResetValue();
        else
            speed.Value = 0;
    }

    public void Damage(int amount)
    {
        health.Value -= amount;
    }

    private void OnHealthValueChange(int value)
    {
        if (value < 0)
        {
            health.ResetValue();
            player.Death();
        }
    }
}
