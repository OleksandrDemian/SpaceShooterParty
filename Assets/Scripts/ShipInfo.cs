public class ShipInfo
{
    public int shipSkin;
    public int health;
    public int damage;
    public int shield;
    public int speed;
    public int abilityLevel;
    public AbilityType abilityType;

    public void InitializeShip(ShipController ship, int color)
    {
        ship.SetHealth(health);
        ship.SetDamage(damage);
        ship.SetSpeed(speed);
        ship.SetShield(shield);
        ship.SetImage(GameManager.ImagePooler.GetShipSkin((shipSkin * 4) + color), color);
        ship.GetPlayer().Write("rc" + color);
        Ability ability;

        switch (abilityType)
        {
            case AbilityType.CIRCLEFIRE:
                //--------------------------ATENTION (abilitylevel for circle fire is always 5)-----------------------------//
                ability = new CircleFire(/*abilityLevel*/ 5, ship);
                break;
            case AbilityType.SHIELDRECOVERY:
                ability = new ShieldRecovery(ship.GetAttribute(AttributeType.SHIELD), 3);
                break;
            case AbilityType.FREEZEENGINE:
                ability = new DisableControllAbility(4, ship);
                break;
            case AbilityType.DESTROYSHIELD:
                ability = new DestroyShieldAbility(2, ship);
                break;
            default:
                return;
        }
        ship.SetAbility(ability);
    }

    public override string ToString()
    {
        return "Ship info: health -> " + health + " damage -> " + damage + " shield -> " + shield + " speed -> " + speed + " ability-> " + abilityType.ToString();
    }
}