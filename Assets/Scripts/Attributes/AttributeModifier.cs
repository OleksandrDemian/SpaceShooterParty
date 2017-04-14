public class AttributeModifier
{
    public ModifierType Type
    {
        get;
        private set;
    }

    public int Value
    {
        get;
        private set;
    }

    public AttributeModifier(ModifierType type, int value)
    {
        Value = value;
        Type = type;
    }

    public void Apply(ref int value)
    {
        //value += Value;
        switch (Type)
        {
            case ModifierType.ADD:
                value += Value;
                return;
            case ModifierType.MULTIPLY:
                value *= Value;
                return;
        }
    }
}