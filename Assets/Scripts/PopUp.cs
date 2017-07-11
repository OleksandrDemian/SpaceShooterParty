using UnityEngine;
using UnityEngine.UI;

public enum PopUpAnimation
{
    UP,
    DOWN,
    RIGHT,
    LEFT,
    GRAVITY
}

public class PopUp : MonoBehaviour, IPoolable {

    private Text text;
    private Color targetColor;
    private float waitTime = 0f;

    private void Start()
    {
        Transform canvas = GameObject.FindGameObjectWithTag("GameCanvas").transform;
        if (canvas == null)
        {
            throw new System.Exception("There is no game canvas!");
        }

        transform.SetParent(canvas);
    }

    public static void ShowText(Vector3 position, string message, float time, Color color, PopUpAnimation animation)
    {
        PopUp popup = GameManager.ObjectPooler.Get<PopUp>();
        popup.Initialize(position, message, time, color, animation);
    }

    public static void ShowText(Vector3 position, string message, float time, Color color)
    {
        PopUp popup = GameManager.ObjectPooler.Get<PopUp>();
        popup.Initialize(position, message, time, color, PopUpAnimation.GRAVITY);
    }

    public static void ShowText(Vector3 position, string message, float time)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, time, Color.white, PopUpAnimation.GRAVITY);
    }

    public static void ShowText(Vector3 position, string message)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, 0, Color.white, PopUpAnimation.GRAVITY);
    }

    public static void ShowText(Vector3 position, string message, PopUpAnimation animation)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, 0, Color.white, animation);
    }

    public static void ShowText(Vector3 position, string message, PopUpAnimation animation, float time)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, time, Color.white, animation);
    }

    public void Initialize(Vector3 position, string message, float waitTime, Color color, PopUpAnimation animation)
    {
        text = GetComponent<Text>();
        text.text = message;
        text.color = Color.clear;
        targetColor = color;
        gameObject.SetActive(true);
        transform.position = position + Vector3.up;
        this.waitTime = waitTime;
        switch (animation)
        {
            case PopUpAnimation.UP:
                StartCoroutine(AnimateDirection(Vector3.up));
                break;
            case PopUpAnimation.DOWN:
                StartCoroutine(AnimateDirection(Vector3.down));
                break;
            case PopUpAnimation.RIGHT:
                StartCoroutine(AnimateDirection(Vector3.right));
                break;
            case PopUpAnimation.LEFT:
                StartCoroutine(AnimateDirection(Vector3.left));
                break;
            case PopUpAnimation.GRAVITY:
                StartCoroutine(AnimateGravity());
                break;
        }
    }

    private System.Collections.IEnumerator AnimateDirection(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction;
        while (text.color.a < 0.99f) {
            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, 10 * Time.deltaTime);
            text.color = Color.LerpUnclamped(text.color, targetColor, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(waitTime);

        while (text.color.a > 0.1f)
        {
            text.color = Color.LerpUnclamped(text.color, Color.clear, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Disable();
    }

    private System.Collections.IEnumerator AnimateGravity()
    {
        Vector3 direction = new Vector3(Random.Range(-2f, 2f), Random.Range(3f, 6f), 0);
        text.color = targetColor;
        while (text.color.a > 0.1f)
        {
            transform.position += (direction * GameTime.TimeScale);
            direction.y -= (GameTime.TimeScale * 8);
            text.color = Color.LerpUnclamped(text.color, Color.clear, GameTime.TimeScale);
            yield return new WaitForEndOfFrame();
        }
        Disable();
    }

    private void Disable()
    {
        ObjectPool.Instance.Add(this);
        this.gameObject.SetActive(false);
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }
}
