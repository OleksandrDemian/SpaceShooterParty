using UnityEngine;
using UnityEngine.UI;

public enum PopUpAnimation
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public class PopUp : MonoBehaviour, IPoolable {

    private Text text;
    private Color targetColor;
    private float waitTime = 0f;

    private void Start()
    {
        this.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }

    public static void ShowText(Vector3 position, string message, float time, Color color, PopUpAnimation animation)
    {
        PopUp popup = GameManager.ObjectPooler.Get<PopUp>();
        popup.Initialize(position, message, time, color, animation);
    }

    public static void ShowText(Vector3 position, string message, float time, Color color)
    {
        PopUp popup = GameManager.ObjectPooler.Get<PopUp>();
        popup.Initialize(position, message, time, color, PopUpAnimation.UP);
    }

    public static void ShowText(Vector3 position, string message, float time)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, time, Color.white, PopUpAnimation.UP);
    }

    public static void ShowText(Vector3 position, string message)
    {
        PopUp popup = ObjectPool.Instance.Get<PopUp>();
        popup.Initialize(position, message, 0, Color.white, PopUpAnimation.UP);
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
                StartCoroutine(AnimateDIrection(Vector3.up));
                break;
            case PopUpAnimation.DOWN:
                StartCoroutine(AnimateDIrection(Vector3.down));
                break;
            case PopUpAnimation.RIGHT:
                StartCoroutine(AnimateDIrection(Vector3.right));
                break;
            case PopUpAnimation.LEFT:
                StartCoroutine(AnimateDIrection(Vector3.left));
                break;
        }
    }

    private System.Collections.IEnumerator AnimateDIrection(Vector3 direction)
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
