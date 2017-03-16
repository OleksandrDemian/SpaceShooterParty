public class ShipInfo
{
    public int shipSkin;
    public int health;
    public int damage;
    public int shield;
    public int speed;
    public AbilityType abilityType;
    public int abilityLevel;

    public void InitializeShip(ShipController ship, int color)
    {
        ship.SetHealth(health);
        ship.SetDamage(damage);
        ship.SetSpeed(speed);
        ship.SetShield(shield);
        ship.SetImage(GameManager.ImagePooler.GetShipSkin((shipSkin * 4) + color));

        Ability ability;

        switch (abilityType)
        {
            case AbilityType.CIRCLEFIRE:
                ability = new CircleFire(ship.GetDamage(), abilityLevel, ship.transform);
                break;
            case AbilityType.SHIELDRECOVERY:
                ability = new ShieldRecovery(ship.GetShield(), abilityLevel);
                break;
            case AbilityType.FREEZEENGINE:
                ability = new EngineFreezAbility(2, ship.transform);
                break;
            case AbilityType.DESTROYSHIELD:
                ability = new DestroyShieldAbility(2, ship.transform);
                break;
            default:
                return;
        }
        ship.SetAbility(ability);
    }

    public override string ToString()
    {
        return "Ship info: health -> " + health + " damage -> " + damage + " shield -> " + shield + " speed -> " + speed;
    }
}