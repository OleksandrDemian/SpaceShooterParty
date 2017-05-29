using UnityEngine;

public class ExplosionParticle : MonoBehaviour {

    private Vector2 targetPosition;
    //private ExplosionManager manager;

	void Start () {
		
	}
	
	void Update () {
        transform.position = Vector3.Lerp(transform.position, targetPosition, GameTime.TimeScale * 10);
	}

    public void SetExplosionManager(ExplosionManager manager)
    {
        //this.manager = manager;
    }

    public void Initialize(Vector3 targetPosition)
    {
        transform.localPosition = Vector3.zero;
        this.targetPosition = transform.position + targetPosition;

        GetComponent<Animation>().Play("ExplosionParticleAnimation");
    }
}
