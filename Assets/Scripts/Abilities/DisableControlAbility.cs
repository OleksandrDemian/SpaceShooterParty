public class DisableControllAbility : Ability
{
    private float time;
    private ShipController parent;

    public DisableControllAbility(float time, ShipController parent)
    {
        this.time = time;
        this.parent = parent;
    }

    public override void Trigger()
    {
        PopUp.ShowText(parent.transform.position, "Disable controll");
        Laser controller = GameManager.ObjectPooler.Get<Laser>();
        if (controller != null) {
            controller.Initialize(parent.transform.position, parent.transform.rotation, new DisableControl(parent, time));
            controller.SetSprite(GameManager.ImagePooler.GetLaserSkin(1));
        }
    }
}