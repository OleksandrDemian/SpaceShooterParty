using System.Collections.Generic;

public delegate void OnValueChange(int value, int oldValue);

public class Attribute
{
    public OnValueChange onValueChange;
    private int value;
    private int defaultValue;
    private int maxValue;
    private AttributeType type;
    private List<AttributeModifier> modifiers = new List<AttributeModifier>();

    public Attribute(AttributeType type, int value) {
        this.value = value;
        maxValue = value;
        defaultValue = value;
        this.type = type;
    }

    public int Value {
        get
        {
            return value;
        }
        set
        {
            int old = this.value;
            this.value = value;

            if (this.value > maxValue)
                this.value = maxValue;

            if (onValueChange != null)
                onValueChange(Value, old);
        }
        /*get {
            int finalValue = value;
            foreach (AttributeModifier modifier in modifiers)
            {
                modifier.Apply(ref finalValue);
            }
            return finalValue;
        }*/
        /*private set {
            this.value = value;
            //Mathf.Clamp(this.value, 0, maxValue);
            if (this.value > maxValue)
                this.value = maxValue;

            if (onValueChange != null)
                onValueChange(Value);
        }*/
    }

    public void ResetValue()
    {
        value = maxValue;
    }

    public void ResetDefaultValue()
    {
        maxValue = defaultValue;
        value = maxValue;
        ClearModifiers();
    }

    public AttributeType Type
    {
        get
        {
            return type;
        }
    }

    public void AddModifier(AttributeModifier modifier)
    {
        int old = Value;
        modifiers.Add(modifier);
        CalculateValue();

        if (onValueChange != null)
            onValueChange(Value, old);
    }

    public void RemoveModifier(AttributeModifier modifier)
    {
        modifiers.Remove(modifier);
        CalculateValue();
    }

    public void ClearModifiers()
    {
        modifiers.Clear();
        CalculateValue();
    }

    public override string ToString()
    {
        return "Value: " + Value + " MaxValue: " + maxValue + " DefaultValue: " + defaultValue;
    }

    private void CalculateValue()
    {
        int currentValuePercent = (value * 100) / maxValue;
        //Debug.Log("Percents: Value: " + value + " max: " + maxValue + " percents: " + currentValuePercent);

        int finalValue = defaultValue;
        int add = 0;
        int mult = 0;

        foreach (AttributeModifier modifier in modifiers)
        {
            //modifier.Apply(ref finalValue);
            switch (modifier.Type)
            {
                case ModifierType.ADD:
                    modifier.Apply(ref add);
                    break;
                case ModifierType.MULTIPLY:
                    modifier.Apply(ref mult);
                    break;
            }
        }

        finalValue += add;
        if (mult != 0)
            finalValue *= mult;
        

        maxValue = finalValue;
        Value = (currentValuePercent * maxValue) / 100;
    }
    /*public void Add(int value)
    {
        this.value += value;
        if (this.value > maxValue)
            this.value = maxValue;

        if (onValueChange != null)
            onValueChange(Value);
    }

    public void Substract(int value)
    {
        this.value -= value;

        if (onValueChange != null)
            onValueChange(Value);
    }*/

    

    /*public void Set(int value)
    {
        this.value = value;

        if (this.value > maxValue)
            this.value = maxValue;

        if (onValueChange != null)
            onValueChange(Value);
    }*/
}