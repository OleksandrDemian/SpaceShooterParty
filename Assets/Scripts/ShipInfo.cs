public class ShipInfo
{
    public int shipSkin;
    public int health;
    public int damage;
    public int shield;
    public int speed;

    public override string ToString()
    {
        return "Ship ingo: health -> " + health + " damage -> " + damage + " shield -> " + shield + " speed -> " + speed;
    }
}