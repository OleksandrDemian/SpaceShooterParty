using UnityEngine;

public enum FireMode
{
    ONESHOT,
    DOUBLESHOT
}

public class Fire
{
    private float lastFire = 0f;
    private float fireRate = 0.3f;
    private FireMode mode = FireMode.ONESHOT;
    private ShipController parent;

    public Fire(ShipController parent, float fireRate)
    {
        this.parent = parent;
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

        switch (mode)
        {
            case FireMode.ONESHOT:
                Laser controller = GameManager.ObjectPooler.Get<Laser>();

                controller.Initialize(parent.transform.position, parent.transform.rotation, new Damage(parent));
                controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                //controller.GetDamage().SetDamageListener(DamageListener);
                parent.GetAudioManager().PlayAudio("Laser");
                lastFire = GameTime.GetTime();
                break;
            case FireMode.DOUBLESHOT:
                Laser controller1 = GameManager.ObjectPooler.Get<Laser>();
                Laser controller2 = GameManager.ObjectPooler.Get<Laser>();

                Damage damage = new Damage(parent);
                Vector3 offset = parent.transform.right * 0.2f;

                controller1.Initialize(parent.transform.position + offset, parent.transform.rotation, damage);
                controller2.Initialize(parent.transform.position - offset, parent.transform.rotation, damage);

                controller1.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                controller2.SetSprite(GameManager.ImagePooler.GetLaserSkin(0));
                parent.GetAudioManager().PlayAudio("Laser");
                lastFire = GameTime.GetTime();
                break;
        }
    }
}
