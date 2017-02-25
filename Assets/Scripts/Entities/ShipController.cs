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

        EnableEngine(false);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationTarget), rotationSpeed.Value * Time.deltaTime);
        GameManager.Instance.CheckPosition(transform);

        if (Input.GetKeyDown(KeyCode.F))
            AbilityTrigger();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.up * speed.Value * Time.deltaTime, ForceMode2D.Force);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetSpeed(int value)
    {
        speed = new Attribute(AttributeType.SPEED, value);
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
        GameObject bullet = GameManager.ObjectPooler.Get(GOType.LASER);
        BulletController controller = bullet.GetComponent<BulletController>();
        controller.Initialize(transform.position, transform.rotation, new Damage(damage.Value));
        controller.SetSprite(GameManager.ObjectPooler.GetLaserSkin(0));
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

    private void EnableEngine(bool action)
    {
        if (action)
            speed.ResetValue();
        else
            speed.Value = 0;

        engineTrail.enabled = speed.Value > 0 ? true : false;
    }

    public void Damage(int amount)
    {
        if (shield.Value > 0)
        {
            shield.Value--;
        }
        else
        {
            health.Value -= amount;
        }
    }

    public void DisableControll(float time) {
        player.Write(Converter.toString(Request.DISABLECONTROLLER) + time);
    }

    private void OnHealthValueChange(int value)
    {
        if (value < 0)
        {
            health.ResetValue();
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