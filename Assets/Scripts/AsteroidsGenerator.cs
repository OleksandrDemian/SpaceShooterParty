using UnityEngine;

public class AsteroidsGenerator : MonoBehaviour {

    private float generate = 5;

	void Start () {
		
	}
	
	void Update () {
        if (Time.time > generate)
        {
            GenerateAsteroid();
            generate += Random.Range(5, 15);
        }
	}

    private void GenerateAsteroid()
    {
        GameObject asteroid = GameManager.ObjectPooler.Get(EntityType.ASTEROID);
        Vector3 position = new Vector3(50, 50, 0);
        asteroid.transform.position = position;
    }
}
