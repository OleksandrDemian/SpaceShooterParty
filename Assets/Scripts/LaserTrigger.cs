using UnityEngine;

/// <summary>
/// Effect used after laser hit something
/// </summary>
public class LaserTrigger : MonoBehaviour, IPoolable {
	
	void Update ()
    {
        transform.localScale -= Vector3.one * GameTime.TimeScale;
        if(transform.localScale.x < 0.1f)
        {
            Disable ();
        }

	}

    public void Initialize(Vector3 position)
    {
        transform.position = position;
        transform.localScale = Vector3.one;
    }

    public GameObject GetGameObject
    {
        get
        {
            return gameObject;
        }
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }
}
