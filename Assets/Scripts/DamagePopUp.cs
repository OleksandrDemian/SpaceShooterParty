using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour, IPoolable {

    private Text text;
    private Color targetColor;

    private void Start()
    {
        text = GetComponent<Text>();
        this.transform.SetParent(FindObjectOfType<Canvas>().transform);
    }

    public void Initialize(Vector3 position, string message, Color color)
    {
        text = GetComponent<Text>();
        text.text = message;
        text.color = Color.clear;
        targetColor = color;
        gameObject.SetActive(true);
        transform.position = position + Vector3.up;
        StartCoroutine(Animate());
    }

    private System.Collections.IEnumerator Animate()
    {
        Vector3 targetPosition = transform.position + Vector3.up;
        while (text.color.a < 0.99f) {
            transform.position = Vector3.LerpUnclamped(transform.position, targetPosition, 10 * Time.deltaTime);
            text.color = Color.LerpUnclamped(text.color, targetColor, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        while (text.color.a > 0.1f)
        {
            text.color = Color.LerpUnclamped(text.color, Color.clear, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Disable();
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }

    public GOType Type
    {
        get
        {
            return GOType.DAMAGEPOPUP;
        }
    }

    public GameObject Get
    {
        get
        {
            return gameObject;
        }
    }
}
