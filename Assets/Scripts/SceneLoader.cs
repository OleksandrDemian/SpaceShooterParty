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
        Color colorI = image.color;
        Color colorT = text.color;

        while (colorI.a != target)
        {
            colorI.a = Mathf.MoveTowards (colorI.a, target, Time.deltaTime * 5);
            colorT.a = Mathf.MoveTowards(colorT.a, target, Time.deltaTime * 5);
            image.color = colorI;
            text.color = colorT;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);

        if (f != null)
            f();

        if(target == 0)
            gameObject.SetActive(false);
    }
}