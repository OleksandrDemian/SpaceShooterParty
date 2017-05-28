using System.Collections.Generic;
using UnityEngine;

public class BonusGenerator
{
    [SerializeField]
    private int minDelay = 10;
    [SerializeField]
    private int maxDelay = 20;

    public BonusGenerator()
    {
        GameTime.Instance.AddTimer(new Timer(Random.Range(minDelay, maxDelay), delegate () { GenerateBonus(); }));
    }

    private void GenerateBonus()
    {
        PowerUp bonus = ObjectPool.Instance.Get<PowerUp>();
        Vector2 bounds = GameManager.Instance.MapBounds - Vector2.one;

        int x = Random.Range((int)-bounds.x, (int)bounds.x);
        int y = Random.Range((int)-bounds.y, (int)bounds.y);

        Debug.Log("Position: " + x + " " + y);
        bonus.transform.position = new Vector2(x, y);

        bool negative = false;
        bonus.AddBonus(GetRandomBonus(ref negative));

        bonus.EnableFollowing(negative);
        GameTime.Instance.AddTimer(new Timer(Random.Range(minDelay, maxDelay), delegate () { GenerateBonus(); }));
    }

    private Bonus GetRandomBonus(ref bool negative)
    {
        int rand = Random.Range(0, 14);

        switch (rand)
        {
            case 1:
                negative = false;
                return new LevelUpBonus();

            case 2:
                negative = false;
                return new SlowDownTimeBonus();

            case 3:
                negative = false;
                return new HealBonus();

            case 4:
                negative = false;
                return new MultiplyDamageBonus(2);

            case 5:
                negative = false;
                return new DestroyShieldsBonus();

            case 6:
                negative = true;
                return new DestroyShieldsBonus(true);

            case 7:
                negative = false;
                return new MultiplyDamageBonus(3);

            case 8:
                negative = false;
                return new CircleFireBonus(4);

            case 9:
                negative = false;
                return new DestroyShieldsBonus(false);

            case 10:
                negative = true;
                return new HealBonus(-20);

            case 11:
                negative = false;
                return new SlowDownTimeBonus(1.3f);

            case 12:
                negative = false;
                return new GenericBonus(delegate(GameObject target)
                {
                    Vector3 position = target.transform.position;
                    PopUp.ShowText(position, "Black hole", 2);
                    GameTime.Instance.AddTimer(new Timer(3, delegate()
                    {
                        BlackHole bh = ObjectPool.Instance.Get<BlackHole>();
                        bh.Initialize(position);
                    }));
                });

            case 13:
                negative = false;
                return new GenericBonus(delegate(GameObject target)
                {
                    Player player = target.GetComponent<ShipController>().GetPlayer();
                    if (player == null)
                        return;

                    PopUp.ShowText(target.transform.position, "Disable players", 1);
                    List<Player> players = GameManager.Instance.GetPlayers();
                    players.Remove(player);

                    for (int i = 0; i < players.Count; i++)
                    {
                        players[i].EnableControll(false);
                    }

                    GameTime.Instance.AddTimer(new Timer(5, delegate()
                    {
                        for (int i = 0; i < players.Count; i++)
                        {
                            players[i].EnableControll(true);
                        }
                    }));
                });

            default:
                negative = false;
                return new CircleFireBonus();
        }
    }
}
