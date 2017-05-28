using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

internal delegate void OnFadingEnd();

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance
    {
        get;
        private set;
    }

    private void Start()
    {
        Instance = this;
        StartCoroutine(Fade(0, null));
    }

    public static void LoadScene(string sceneName)
    {
        Instance.gameObject.SetActive(true);
        Instance.StartCoroutine(Instance.Fade(1, delegate ()
        {
            SceneManager.LoadScene(sceneName);
        }));
    }

    public static void LoadScene(int sceneIndex)
    {
        Instance.gameObject.SetActive(true);
        Instance.StartCoroutine(Instance.Fade(1, delegate ()
        {
            SceneManager.LoadScene(sceneIndex);
        }));
    }

    private IEnumerator Fade(int target, OnFadingEnd f)
    {
        yield return new WaitForSeconds(1 - target);

        Image image = GetComponent<Image>();
        Text text = GetComponentInChildren<Text>();
        Color color = image.color;

        while (color.a != target)
        {
            color.a = Mathf.MoveTowards (color.a, target, Time.deltaTime * 5);
            image.color = color;
            text.color = color;
            yield return new WaitForEndOfFrame();
        }

        if (f != null)
            f();

        gameObject.SetActive(false);
    }
}