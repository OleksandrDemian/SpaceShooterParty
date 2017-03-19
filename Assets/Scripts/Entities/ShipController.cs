using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour, IDamagable
{
    /*private Attribute speed;
    private Attribute rotationSpeed;
    private Attribute health;
    private Attribute damage;
    private Attribute shield;*/
    private int speed = 0;
    private int rotationSpeed = 10;
    private List<Attribute> attributes = new List<Attribute>();

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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationTarget), rotationSpeed * GameTime.TimeScale);
        calculateMoveDirection(transform.up * speed);
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
#endif
    }

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
        attributes.Add(new Attribute(AttributeType.SPEED, value/100));
    }

    public void SetShield(int value)
    {
        Attribute shield = new Attribute(AttributeType.SHIELD, value);
        shield.onValueChange = OnShieldValueChange;
        attributes.Add(shield);
    }

    public void SetHealth(int value)
    {
        Attribute health = new Attribute(AttributeType.HEALTH, value);
        health.onValueChange = OnHealthValueChange;
        attributes.Add(health);
    }

    public void SetDamage(int value)
    {
        attributes.Add(new Attribute(AttributeType.DAMAGE, value));
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

    /*public Attribute GetDamage()
    {
        return damage;
    }

    public Attribute GetShield()
    {
        return shield;
    }*/

    public Attribute GetAttribute(AttributeType type)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i].Type == type)
                return attributes[i];
        }
        return null;
    }

    public void Fire()
    {
        Attribute damage = GetAttribute(AttributeType.DAMAGE);
        if (damage == null)
            return;

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
        /*health.ResetValue();
        shield.ResetValue();
        damage.ResetValue();*/
        for (int i = 0; i < attributes.Count; i++)
            attributes[i].ResetValue();
        EnableEngine(false);
    }

    public void SetRotationTarget(int rotationTarget)
    {
        this.rotationTarget = rotationTarget;
    }

    private void EnableEngine(bool action)
    {
        Attribute speedAttr = GetAttribute(AttributeType.SPEED);
        if (action)
            speedAttr.ResetValue();
        else
            speedAttr.Value = 0;

        speed = speedAttr.Value;
        engineTrail.enabled = speed > 0 ? true : false;
    }

    public void Damage(int amount, OnDead onDead)
    {
        Attribute shield = GetAttribute(AttributeType.SHIELD);
        DamagePopUp popup = GameManager.ObjectPooler.Get(EntityType.DAMAGEPOPUP).GetComponent<DamagePopUp>();
        if (shield.Value > 0)
        {
            shield.Value--;
            popup.Initialize(transform.position, "1", Color.blue);
        }
        else
        {
            Attribute health = GetAttribute(AttributeType.HEALTH);
            health.Value -= amount;
            if (health.Value < 0)
            {
                if(onDead != null)
                    onDead(gameObject);
            }
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