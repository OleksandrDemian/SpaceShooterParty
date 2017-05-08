﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Transform popUpPosition;
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

    public void RemoveName(string name)
    {
        names.Remove(name);
        UpdateNames();
    }

    public void StartGame()
    {
        //Count must be > 1!!!!!
        if (names.Count > 0)
        {
            Server.Instance.Stop();
            SceneLoader.LoadScene("Game");
        }
        else
        {
            Vector3 position;
            if (popUpPosition != null)
            {
                position = popUpPosition.position;
            }
            else
            {
                Debug.Log("There is no indicator of popUp's position");
                position = new Vector3(0, -8, 0);
            }

            PopUp.ShowText(position, "There must be at minimum 2 players", 1);
        }
    }

    public void ExitGame()
    {
        Server.Instance.Stop();
        Application.Quit();
    }

    private void UpdateNames()
    {
        for (int i = 0; i < playersNames.Length; i++)
        {
            playersNames[i].text = (i < names.Count ? names[i] : "Empty");
        }
    }
}
