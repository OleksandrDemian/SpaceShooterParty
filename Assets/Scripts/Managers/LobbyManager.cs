using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Text[] playersNames;
    [SerializeField]
    private Text ipText;
    private List<string> names = new List<string>();
    public static LobbyManager Instance
    {
        get;
        private set;
    }

	void Start ()
    {
        Instance = this;
        ipText.text = "IP: " + Server.GetLocalIPAddress();
	}
	
	void Update ()
    {

    }

    public void AddName(string name)
    {
        names.Add(name);
        UpdateNames();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private void UpdateNames()
    {
        for (int i = 0; i < playersNames.Length; i++)
        {
            playersNames[i].text = (i < names.Count ? names[i] : "Empty");
        }
    }
}
