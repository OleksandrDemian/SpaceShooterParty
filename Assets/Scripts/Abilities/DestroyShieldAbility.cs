using UnityEngine;

    class DestroyShieldAbility : Ability
{
    private int amount;
    private Transform transform;

    public DestroyShieldAbility(int amount, Transform transform)
    {
        this.amount = amount;
        this.transform = transform;
    }

    public override void Trigger()
    {
        GameObject laser = GameManager.ObjectPooler.Get(GOType.LASER) as GameObject;
        BulletController controller = laser.GetComponent<BulletController>();
        if (controller != null)
        {
            controller.Initialize(transform.position, transform.rotation, new DestroyShield(amount, 5));
            controller.SetSprite(GameManager.ObjectPooler.GetLaserSkin(2));
        }
    }
}
