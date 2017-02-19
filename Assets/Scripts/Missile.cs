using UnityEngine;

class Missile : MonoBehaviour, IDamagable, IPoolable
{
    private Transform target;

    private void Start()
    {

    }

    private void Update()
    {
        //DA FARE
    }

    public void Initialize()
    {

    }

    public GameObject Get
    {
        get
        {
            return gameObject;
        }
    }

    public GOType Type
    {
        get
        {
            return GOType.MISSILE;
        }
    }

    public void Damage(int amount)
    {
        Disable();
    }

    private void Disable()
    {
        GameManager.ObjectPooler.Add(this);
        this.gameObject.SetActive(false);
    }
}