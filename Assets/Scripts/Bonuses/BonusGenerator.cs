using UnityEngine;

public class BonusGenerator : MonoBehaviour
{
    private float generate = 10;

    private void Start()
    {
        generate = Random.Range(20, 40);
    }

    private void Update()
    {
        if (Time.time > generate)
        {
            GenerateBonus();
            generate += Random.Range(20, 40);
        }
    }

    private void GenerateBonus()
    {
        PowerUp bonus = ObjectPool.Instance.Get<PowerUp>();
        int x = Random.Range(0, 22);
        int y = Random.Range(0, 13);
        bonus.transform.position = new Vector2(x, y);
        bonus.AddBonus(GetRandomBonus());
        //bonus.EnableFollowing(true);
    }

    private Bonus GetRandomBonus()
    {
        int rand = Random.Range(0, 9);

        switch (rand)
        {
            case 1:
                return new LevelUpBonus();

            case 2:
                return new CircleFireBonus();

            case 3:
                return new HealBonus();

            case 4:
                return new DoubleDamageBonus();

            case 5:
                return new DestroyShieldsBonus();

            default:
                return new SlowDownTimeBonus();
        }
    }
}
