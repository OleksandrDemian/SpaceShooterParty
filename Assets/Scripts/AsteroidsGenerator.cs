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
        //GameObject asteroid = GameManager.ObjectPooler.Get(EntityType.ASTEROID);
        //GameObject asteroid = GameManager.ObjectPooler.Get<Asteroid>() as GameObject;
        Asteroid asteroid = GameManager.ObjectPooler.Get<Asteroid>();
        asteroid.Initialize();
    }
}