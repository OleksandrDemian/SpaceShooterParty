﻿public delegate void OnValueChange(int value);

public class Attribute
{
    public OnValueChange onValueChange;
    private int value;
    private int maxValue;
    private AttributeType type;

    public Attribute(AttributeType type, int value) {
        this.value = value;
        maxValue = value;
        this.type = type;
    }

    public int Value {
        get { return value; }
        set {
            this.value = (value > maxValue ? maxValue : value);
            if (onValueChange != null) onValueChange(value);
        }
    }

    public AttributeType Type {
        get { return type; }
    }

    public void ResetValue() {
        value = maxValue;
    }
}