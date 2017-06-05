using System;

class ShieldRecovery : Ability
{
    private Attribute playerShield;
    private int amount = 1;

    public ShieldRecovery(Attribute attribute, int amount)
    {
        if(attribute.Type != AttributeType.SHIELD)
            throw new ArgumentException("Attribute must be of type shield");

        playerShield = attribute;
        this.amount = amount;
    }

    public override void Trigger()
    {
        playerShield.Value += amount;
    }

    public override int RechargeTime
    {
        get
        {
            return 6;
        }
    }
}
