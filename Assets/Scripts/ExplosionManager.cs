using System.Collections;
using UnityEngine;

public class ExplosionManager : MonoBehaviour, IPoolable
{
    private ExplosionParticle[] particles;

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }

    public void Initialize(Vector3 position)
    {
        transform.position = position;

        if(particles == null)
            particles = GetComponentsInChildren<ExplosionParticle>();

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Initialize(new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0));
        }

        StartCoroutine(DisableWait());
    }

    private IEnumerator DisableWait()
    {
        float timer = 0;
        while (timer < 0.7f)
        {
            timer += GameTime.TimeScale;
            yield return new WaitForEndOfFrame();
        }
        Disable();
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        gameObject.SetActive(false);
    }
	
	private void Update ()
    {
		
	}
}
