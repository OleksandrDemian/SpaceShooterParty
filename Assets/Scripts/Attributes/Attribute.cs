using UnityEngine;

public delegate void OnValueChange(int value);

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
            this.value = value;
            Mathf.Clamp(this.value, 0, maxValue);

            if (onValueChange != null)
                onValueChange(Value);
        }
    }

    public AttributeType Type {
        get { return type; }
    }

    public void ResetValue() {
        Value = maxValue;
    }
}