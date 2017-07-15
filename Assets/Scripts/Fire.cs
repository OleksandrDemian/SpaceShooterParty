using System.Collections;
using UnityEngine;

public enum FireMode
{
    ONESHOT,
    DOUBLESHOT,
    SERIALFIRE
}

public class Fire
{
    private float lastFire = 0f;
    private float fireRate = 0.3f;
    private FireMode mode = FireMode.ONESHOT;
    private IDamageListener listener;
    private AudioManager audio;

    public Fire(IDamageListener listener, float fireRate)
    {
        this.listener = listener;
        audio = listener.GetGameObject.GetComponent<AudioManager>();
        this.fireRate = fireRate;
        lastFire = GameTime.GetTime();
    }

    public void SetMode(FireMode mode)
    {
        this.mode = mode;
    }

    public void SetFireRate(float fireRate)
    {
        this.fireRate = fireRate;
    }

    public void Trigger()
    {
        if (GameTime.GetTime() < lastFire + fireRate)
            return;

        Transform transform = listener.GetGameObject.transform;

        if(audio != null)
            audio.PlayAudio("Laser");

        lastFire = GameTime.GetTime();

        switch (mode)
        {
            case FireMode.ONESHOT:
                Laser controller = GameManager.ObjectPooler.Get<Laser>();

                controller.Initialize(transform.position, transform.rotation, new Damage(listener));
                controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                break;
            case FireMode.DOUBLESHOT:
                Laser controller1 = GameManager.ObjectPooler.Get<Laser>();
                Laser controller2 = GameManager.ObjectPooler.Get<Laser>();

                Damage damage = new Damage(listener);
                Vector3 offset = transform.right * 0.2f;

                controller1.Initialize(transform.position + offset, transform.rotation, damage);
                controller2.Initialize(transform.position - offset, transform.rotation, damage);

                controller1.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                controller2.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                break;
            case FireMode.SERIALFIRE:
                Laser controller4 = GameManager.ObjectPooler.Get<Laser>();

                controller4.Initialize(transform.position, transform.rotation, new Damage(listener));
                controller4.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));

                GameTime.Instance.AddTimer(new Timer(0.1f, delegate()
                {
                    Laser controller3 = GameManager.ObjectPooler.Get<Laser>();

                    controller3.Initialize(transform.position, transform.rotation, new Damage(listener));
                    controller3.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                }));

                GameTime.Instance.AddTimer(new Timer(0.2f, delegate ()
                {
                    Laser controller3 = GameManager.ObjectPooler.Get<Laser>();

                    controller3.Initialize(transform.position, transform.rotation, new Damage(listener));
                    controller3.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                }));
                break;
        }
    }
}
