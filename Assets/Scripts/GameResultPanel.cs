using System;
using UnityEngine;

public class GameResultPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject playerInfoPrephab;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void Show(Player[] players)
    {
       /* Array.Sort(players, delegate(Player x, Player y) {
            return x.kill.CompareTo(y.kill);
        });*/

        Array.Sort(players, (y, x) => x.kill.CompareTo(y.kill));

        for (int i = 0; i < players.Length; i++) {
            Debug.Log("Player: " + players[i].Name);
            GameObject temp = Instantiate(playerInfoPrephab);
            temp.transform.SetParent(FindObjectOfType<Canvas>().transform);
            temp.GetComponent<Podium>().SetUp(i, players[i].Name);
        }
    }
}