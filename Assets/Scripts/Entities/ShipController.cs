using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioManager))]
public class ShipController : MonoBehaviour, IDamagable
{
    private int speed = 0;
    private int rotationSpeed = 4;
    private List<Attribute> attributes = new List<Attribute>();

    private Ability ability;
    private Vector3 moveDirection = Vector3.zero;

    private int rotationTarget = 0;
    private bool engineEnabled;
    private Rigidbody2D rb;
    private Player player;

    private SpriteRenderer engineTrail;
    private SpriteRenderer shieldSprite;

    private AudioManager audioManager;

    //TEMP
    private float lastFireTime;
    private float lastAbilityTime;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        audioManager = GetComponent<AudioManager>();

        engineTrail = transform.Find("Trail").GetComponent<SpriteRenderer>();
        shieldSprite = transform.Find("Shield").GetComponent<SpriteRenderer>();

        EnableEngine(false);
        lastFireTime = GameTime.GetTime();
        lastAbilityTime = GameTime.GetTime();

        if (GameInfo.Instance.shieldsDisabled)
        {
            GetAttribute(AttributeType.SHIELD).AddModifier(new AttributeModifier(ModifierType.MULTIPLY, 0));
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rotationTarget), rotationSpeed * GameTime.TimeScale);
        CalculateMoveDirection(transform.up * speed);
        GameManager.Instance.CheckPosition(transform);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveDirection * GameTime.TimeScale);
    }
    
    private void CalculateMoveDirection(Vector3 dir)
    {
        moveDirection = Vector3.Lerp(moveDirection, dir, GameTime.TimeScale);
    }

    //UNUSED
    public void AddAttribute(AttributeType type, int value)
    {
        Attribute attribute = new Attribute(type, value);
        attributes.Add(attribute);
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public Player GetPlayer()
    {
        return player;
    }

    //SAME METHOD
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
    //END SAME METHOD

    public void SetImage(Sprite sprite)
    {
        //Debug.Log(sprite.name);
        //Set space ship's skin
        transform.Find("Ship").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
    }

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
        if (GameTime.GetTime() < lastFireTime + 0.5f)
            return;

        Attribute damage = GetAttribute(AttributeType.DAMAGE);
        if (damage == null)
            return;

        Laser controller = GameManager.ObjectPooler.Get<Laser>();

        controller.Initialize(transform.position, transform.rotation, new Damage(damage.Value));
        controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
        controller.GetDamage().SetParentShip(this);
        audioManager.PlayAudio("Laser");
        lastFireTime = GameTime.GetTime();
    }

    public void AbilityTrigger()
    {
        if (GameTime.GetTime() < lastAbilityTime + 5)
            return;

        ability.Trigger();
        lastAbilityTime = GameTime.GetTime();
    }

    public void TurnRight()
    {
        rotationTarget -= 3;
    }

    public void TurnLeft()
    {
        rotationTarget += 3;
    }

    public void EngineTrigger()
    {
        engineEnabled = !engineEnabled;
        EnableEngine(engineEnabled);
    }

    public void ResetAttributes()
    {
        for (int i = 0; i < attributes.Count; i++)
            attributes[i].ResetValue();
        EnableEngine(false);
        moveDirection = Vector3.zero;
        rotationTarget = (int)transform.rotation.eulerAngles.z;
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

    public void Damage(int amount, ShipController ship)
    {
        Attribute shield = GetAttribute(AttributeType.SHIELD);
        
        if (shield.Value > 0)
        {
            shield.Value--;
            //PopUp.ShowText(transform.position, "1", 0, Color.blue);
        }
        else
        {
            Attribute health = GetAttribute(AttributeType.HEALTH);
            health.Value -= amount;
            if (health.Value < 0)
            {
                if(ship != null)
                    ship.GetPlayer().Kill(gameObject);
            }
            //PopUp.ShowText(transform.position, amount.ToString(), 0, Color.red);
        }
    }

    /*
    public void DisableControll() {
        //player.Write(Converter.toString(Request.DISABLECONTROLLER) + time);
        player.EnableControll(false);
    }
    */

    private void OnHealthValueChange(int value, int oldValue)
    {
        int delta = value - oldValue;
        player.OnShipValueChange(AttributeType.HEALTH, value);

        if(delta < 0)
            PopUp.ShowText(transform.position, "Health: " + delta, 0, Color.red);
        else
            PopUp.ShowText(transform.position, "Health: " + delta, 0, Color.white);

        if (value < 0)
        {
            player.Death();
        }
    }

    private void OnShieldValueChange(int value, int oldValue)
    {
        int delta = value - oldValue;
        player.OnShipValueChange(AttributeType.SHIELD, value);

        if (delta < 0)
            PopUp.ShowText(transform.position, "Shield: " + delta, 0, Color.blue, PopUpAnimation.LEFT);
        else
            PopUp.ShowText(transform.position, "Shield: " + delta, 0, Color.white, PopUpAnimation.RIGHT);

        if (value < 1)
            shieldSprite.enabled = false;
        else
            shieldSprite.enabled = true;
    }
}