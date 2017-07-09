using UnityEngine;

public abstract class Bonus
{
    public abstract void Trigger(GameObject target);

    public static PopUpAnimation GetAnimation(Vector3 position)
    {
        float y = GameManager.Instance.MapBounds.y / 2;
        if (position.y > y)
            return PopUpAnimation.DOWN;
        return PopUpAnimation.UP;
    }
}