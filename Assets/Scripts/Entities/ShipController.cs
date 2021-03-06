﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioManager))]
public class ShipController : MonoBehaviour, IDamagable, IBlackHoleAttractable, IDamageListener
{
    private int speed = 0;
    private int rotationSpeed = 4;
    private bool isUndead = false;
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
    private Animation anim;

    //TEMP
    private Fire fireManager;
    private float lastAbilityTime;

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();

        audioManager = GetComponent<AudioManager>();
        audioManager.CheckAudio();

        anim = GetComponent<Animation>();

        engineTrail = transform.Find("Trail").GetComponent<SpriteRenderer>();
        shieldSprite = transform.Find("Shield").GetComponent<SpriteRenderer>();

        EnableEngine(false);

        fireManager = new Fire(this, 0.3f);
        lastAbilityTime = GameTime.GetTime();

        if (!GameInfo.Instance.ShieldsEnabled)
        {
            GetAttribute(AttributeType.SHIELD).AddModifier(new AttributeModifier(ModifierType.MULTIPLY, 0));
            Debug.Log("Shield: " + GetAttribute(AttributeType.SHIELD).ToString());
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

    public Fire GetFire()
    {
        return fireManager;
    }

    public void EnableCollider(bool action)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
            return;

        if (!action)
            anim.Play("FadeIn");

        isUndead = !action;
        col.enabled = action;
    }

    public void SetDamage(int value)
    {
        attributes.Add(new Attribute(AttributeType.DAMAGE, value));
    }
    //END SAME METHOD

    public void SetImage(Sprite sprite, int number)
    {
        //Debug.Log(sprite.name);
        //Set space ship's skin
        SpriteRenderer ren = transform.Find("Ship").GetComponent<SpriteRenderer>();
        ren.sortingOrder = number + 10;
        ren.sprite = sprite;
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
        fireManager.Trigger();
        /*
        if (GameTime.GetTime() < lastFireTime + 0.3f)
            return;

        Attribute damage = GetAttribute(AttributeType.DAMAGE);
        if (damage == null)
            return;

        Laser controller = GameManager.ObjectPooler.Get<Laser>();

        controller.Initialize(transform.position, transform.rotation, new Damage(this));
        controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
        //controller.GetDamage().SetDamageListener(DamageListener);
        audioManager.PlayAudio("Laser");
        lastFireTime = GameTime.GetTime();
        */
    }

    public AudioManager GetAudioManager()
    {
        return audioManager;
    }

    public void DamageListener(DamageEvents result, GameObject target)
    {

        if (target == gameObject)
            return;

        player.ShootResult(result);
    }

    public void AbilityTrigger()
    {
        if (GameTime.GetTime() < lastAbilityTime + ability.RechargeTime)
            return;

        audioManager.PlayAudio("laser5");
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

    public void Damage(int amount, IDamageListener listener)
    {
        Attribute shield = GetAttribute(AttributeType.SHIELD);
        
        if (shield.Value > 0)
        {
            if(listener != null)
                listener.DamageListener(DamageEvents.HIT, gameObject);

            shield.Value--;
            //PopUp.ShowText(transform.position, "1", 0, Color.blue);
        }
        else
        {
            Attribute health = GetAttribute(AttributeType.HEALTH);
            health.Value -= amount;

            if (listener == null)
                return;

            if (health.Value < 1)
            {
                listener.DamageListener(DamageEvents.KILL, gameObject);
            }
            else
            {
                listener.DamageListener(DamageEvents.HIT, gameObject);
            }
            //PopUp.ShowText(transform.position, amount.ToString(), 0, Color.red);
        }
    }

    public void Attract(Vector3 toPosition)
    {
        if (isUndead)
            return;

        Vector3 direction = toPosition - transform.position;
        float distance = Vector3.Distance(transform.position, toPosition);
        Vector3 temp = (direction.normalized) / distance * 25;
        moveDirection += temp * GameTime.TimeScale;
    }

    public int GetDamage()
    {
        return GetAttribute(AttributeType.DAMAGE).Value;
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

        PopUp.ShowText(transform.position, delta.ToString(), 0, Color.white);

        if (value < 1)
        {
            ExplosionManager expManager = ObjectPool.Instance.Get<ExplosionManager>();
            expManager.Initialize(transform.position, 10);
            player.Death();
        }
    }

    private void OnShieldValueChange(int value, int oldValue)
    {
        int delta = value - oldValue;
        player.OnShipValueChange(AttributeType.SHIELD, value);

        PopUp.ShowText(transform.position, delta.ToString(), 0, Color.cyan);

        if (value < 1)
            shieldSprite.enabled = false;
        else
            shieldSprite.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.transform.tag == "Player")
            Physics2D.IgnoreCollision(collider.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
}