using UnityEngine;

public class AsteroidsGenerator : MonoBehaviour
{
    private float generate = 1;
	
	void Update ()
    {
        if (Time.time > generate)
        {
            GenerateAsteroid();
            generate = Time.time + Random.Range(5, 10);
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