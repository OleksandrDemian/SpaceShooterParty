using UnityEngine;

public class StarColorController : MonoBehaviour
{
    private Color targetColor = Color.white;
    private SpriteRenderer sprite;
    private bool up = false;
    private int colorChangeSpeed = 4;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        colorChangeSpeed = Random.Range(1, 4);
    }

    private void Update()
    {
        if (up)
            targetColor.a += GameTime.TimeScale/colorChangeSpeed;
        else
            targetColor.a -= GameTime.TimeScale/colorChangeSpeed;
        sprite.color = targetColor;

        if (targetColor.a > 1 || targetColor.a < 0.2f)
            up = !up;
    }

}