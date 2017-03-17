using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour, IDamagable
{
    private Attribute speed;
    private Attribute rotationSpeed;
    private Attribute health;
    private Attribute damage;
    private Attribute shield;

    private Ability ability;
    private Vector3 moveDirection = Vector3.zero;

    private int rotationTarget = 0;
    private bool engineEnabled;
    private Rigidbody2D rb;
    private Player player;

    private SpriteRenderer engineTrail;
    private SpriteRenderer shieldSprite;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        engineTrail = transform.Find("Trail").GetComponent<SpriteRenderer>();
        shieldSprite = transform.Find("Shield").GetComponent<SpriteRenderer>();

        rotationSpeed = new Attribute(AttributeType.ROTATIONSPEED, 10);
/*
#if UNITY_EDITOR
        SetDamage(10);
        SetHealth(20);
        SetShield(3);
        SetSpeed(500);
#endif
*/
        EnableEngine(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationTarget), rotationSpeed.Value * GameTime.TimeScale);
        calculateMoveDirection(transform.up * speed.Value);
        GameManager.Instance.CheckPosition(transform);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            TurnLeft();

        if (Input.GetKeyDown(KeyCode.D))
            TurnRight();

        if (Input.GetKeyDown(KeyCode.F))
            Fire();

        if (Input.GetKeyDown(KeyCode.W))
            EngineTrigger();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameTime.Instance.SetTimeScaleTarget(slow ? 0.1f : 1);
            slow = !slow;
        }
#endif
    }

    private bool slow = false;

    private void FixedUpdate()
    {
        //rb.AddForce(transform.up * speed.Value * GameTime.TimeScale);

        rb.MovePosition(transform.position + moveDirection * GameTime.TimeScale);
    }
    
    private void calculateMoveDirection(Vector3 dir)
    {
        moveDirection = Vector3.Lerp(moveDirection, dir, GameTime.TimeScale);
        Debug.Log(moveDirection);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetSpeed(int value)
    {
        speed = new Attribute(AttributeType.SPEED, value/100);
    }

    public void SetShield(int value)
    {
        shield = new Attribute(AttributeType.SHIELD, value);
        shield.onValueChange = OnShieldValueChange;
    }

    public void SetHealth(int value)
    {
        health = new Attribute(AttributeType.HEALTH, value);
        health.onValueChange = OnHealthValueChange;
    }

    public void SetDamage(int value)
    {
        damage = new Attribute(AttributeType.DAMAGE, value);
    }

    public void SetImage(Sprite sprite)
    {
        Debug.Log(sprite.name);
        transform.Find("Ship").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
    }

    public Attribute GetDamage()
    {
        return damage;
    }

    public Attribute GetShield()
    {
        return shield;
    }

    public void Fire()
    {
        GameObject bullet = GameManager.ObjectPooler.Get(EntityType.LASER);
        BulletController controller = bullet.GetComponent<BulletController>();
        controller.Initialize(transform.position, transform.rotation, new Damage(damage.Value));
        controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
        controller.GetDamage().SetOnDeadCallback(player.Kill);
    }

    public void AbilityTrigger()
    {
        ability.Trigger();
    }

    public void TurnRight()
    {
        rotationTarget -= 30;
    }

    public void TurnLeft()
    {
        rotationTarget += 30;
    }

    public void EngineTrigger() {
        engineEnabled = !engineEnabled;
        EnableEngine(engineEnabled);
    }

    public void ResetAttributes()
    {
        health.ResetValue();
        shield.ResetValue();
        damage.ResetValue();
        EnableEngine(false);
    }

    public void SetRotationTarget(int rotationTarget)
    {
        this.rotationTarget = rotationTarget;
    }

    private void EnableEngine(bool action)
    {
        if (action)
            speed.ResetValue();
        else
            speed.Value = 0;

        engineTrail.enabled = speed.Value > 0 ? true : false;
    }

    public void Damage(int amount, OnDead onDead)
    {
        DamagePopUp popup = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP).GetComponent<DamagePopUp>();
        if (shield.Value > 0)
        {
            shield.Value--;
            popup.Initialize(transform.position, "1", Color.blue);
        }
        else
        {
            health.Value -= amount;
            if (health.Value < 0)
                onDead(gameObject);
            popup.Initialize(transform.position, amount.ToString(), Color.red);
        }
    }

    public void DisableControll(float time) {
        player.Write(Converter.toString(Request.DISABLECONTROLLER) + time);
    }

    private void OnHealthValueChange(int value)
    {
        if (value < 0)
        {
            player.Death();
        }
    }

    private void OnShieldValueChange(int value)
    {
        if (value < 1)
            shieldSprite.enabled = false;
        else
            shieldSprite.enabled = true;
    }
}