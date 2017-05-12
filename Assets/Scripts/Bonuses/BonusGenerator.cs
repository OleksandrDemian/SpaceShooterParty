using UnityEngine;

public class BonusGenerator
{
    public BonusGenerator()
    {
        GameTime.Instance.AddTimer(new Timer(Random.Range(15, 25), delegate () { GenerateBonus(); }));
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
        GameTime.Instance.AddTimer(new Timer(Random.Range(15, 30), delegate () { GenerateBonus(); }));
    }

    private Bonus GetRandomBonus(ref bool negative)
    {
        int rand = Random.Range(0, 10);

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

            default:
                negative = false;
                return new CircleFireBonus();
        }
    }
}
