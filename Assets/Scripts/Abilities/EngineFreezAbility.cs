using UnityEngine;

class EngineFreezAbility : Ability
{
    private float time;
    private Transform transform;

    public EngineFreezAbility(float time, Transform transform)
    {
        this.time = time;
        this.transform = transform;
    }

    public override void Trigger()
    {
        GameObject laser = GameManager.ObjectPooler.Get(EntityType.LASER) as GameObject;
        BulletController controller = laser.GetComponent<BulletController>();
        if (controller != null) {
            controller.Initialize(transform.position, transform.rotation, new FreezeEngine(2, time));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(1));
        }
    }
}