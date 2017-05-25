using UnityEngine;

public delegate void GenericBonusMethod(GameObject target);

public class GenericBonus : Bonus
{
    private GenericBonusMethod method;

    public GenericBonus(GenericBonusMethod method)
    {
        this.method = method;
    }

    public override void Trigger(GameObject target)
    {
        method(target);
    }
}
