using System;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject playerInfoPrephab;
    [SerializeField]
    private GameObject lobbyButton;

    private void Start() { }

    private void Update() { }

    public void Show(Player[] players)
    {
        Array.Sort(players, (y, x) => x.kill.CompareTo(y.kill));

        int i;
        for (i = 0; i < players.Length; i++) {
            Debug.Log("Player: " + players[i].Name);
            GameObject temp = Instantiate(playerInfoPrephab);
            temp.transform.SetParent(FindObjectOfType<Canvas>().transform);
            temp.GetComponent<Podium>().SetUp(i, players[i].Name + ": " + players[i].kill);
        }

        lobbyButton = Instantiate(lobbyButton);
        lobbyButton.transform.SetParent(FindObjectOfType<Canvas>().transform);
        lobbyButton.GetComponent<Button>().onClick.AddListener(GameManager.Instance.LoadLobbyScreen);
        lobbyButton.GetComponent<RectTransform>().localPosition = new Vector2(0, 100 - (40 * i++));
    }
}