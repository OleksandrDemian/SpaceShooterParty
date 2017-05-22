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
        Transform canvasParent = GameObject.FindGameObjectWithTag("GameCanvas").transform;
        if (canvasParent == null)
            throw new System.Exception("There is no game canvas!");

        int i;
        for (i = 0; i < players.Length; i++) {
            GameObject temp = Instantiate(playerInfoPrephab);
            temp.transform.SetParent(canvasParent);
            temp.GetComponent<Podium>().SetUp(i, players[i].Name + ": " + players[i].kill);
        }

        lobbyButton = Instantiate(lobbyButton);
        lobbyButton.transform.SetParent(canvasParent);
        lobbyButton.GetComponent<Button>().onClick.AddListener(GameManager.Instance.LoadLobbyScreen);
        lobbyButton.GetComponent<RectTransform>().localPosition = new Vector2(0, 100 - (60 * i++));
    }
}