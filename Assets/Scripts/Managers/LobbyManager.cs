﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPool))]
public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Transform popUpPosition;
    [SerializeField]
    private UserTextManager[] playersNames;
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
        string ip = ConnectionManager.GetLocalIP();

        if (ip == "")
            ip = "*" + Server.GetLocalIPAddress();

        ipText.text = "IP: " + ip;
        UpdateNames();
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
        int min = 1;
#if UNITY_EDITOR
        min = -1;
#endif
        //Count must be > 1!!!!!
        if (names.Count > min)
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
                Debug.LogError ("There is no indicator of popUp's position");
                position = new Vector3(0, -8, 0);
            }

            PopUp.ShowText(position, "There must be at least 2 players", PopUpAnimation.UP, 1);
        }
    }

    public void ExitGame()
    {
        Server.Instance.Stop();
        Application.Quit();
    }

    private void UpdateNames()
    {
        if (names.Count < 1)
        {
            playersNames[0].SetUserName("There is no players");
            return;
        }

        for (int i = 0; i < playersNames.Length; i++)
        {
            playersNames[i].SetUserName((i < names.Count) ? names[i] : "");
        }
    }
}
