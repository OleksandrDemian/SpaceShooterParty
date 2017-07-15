using UnityEngine;

public class AsteroidsGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(1, 20)]
    private int generatingRate = 5;
    private float generateAt = 1;
	
	void Update ()
    {
        if (GameTime.GetTime() > generateAt)
        {
            GenerateAsteroid();
            generateAt = GameTime.GetTime() + Random.Range(generatingRate, generatingRate + 5);
        }
	}

    private void GenerateAsteroid()
    {
        int random = Random.Range(0, 100);

        if (random > 20)
        {
            Asteroid asteroid = GameManager.ObjectPooler.Get<Asteroid>();
            asteroid.Initialize();
        }
        else
        {
            Turret turret = ObjectPool.Instance.Get<Turret>();
            turret.Initialize(GameManager.Instance.GetRandomPoint());
        }
    }
}