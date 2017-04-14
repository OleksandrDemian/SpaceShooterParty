using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefs;

    private List<IPoolable> poolable;

    /*
    [SerializeField]
    private GameObject shipPrephab;
    [SerializeField]
    private GameObject asteroidPrephab;
    [SerializeField]
    private GameObject laserPrephab;
    [SerializeField]
    private GameObject damagePopUpPrephab;
    */

    public static ObjectPool Instance
    {
        get;
        private set;
    }

    private void Awake() {
        poolable = new List<IPoolable>();
        Instance = this;
    }

    public void Add(IPoolable obj)
    {
        poolable.Add(obj);
    }

    public T Get<T>()
    {
        //Debug.Log("Request: " + typeof(T));
        for (int i = 0; i < poolable.Count; i++)
        {
            if(poolable[i] is T)
            {
                IPoolable go = poolable[i];
                go.GetGameObject.SetActive(true);
                //GameObject go = poolable[i].GetGameObject;
                //go.SetActive(true);
                poolable.Remove(go);
                //Debug.Log("Had!");
                return (T)go;
            }
        }

        for (int i = 0; i < prefs.Length; i++)
        {
            if (prefs[i].GetComponent<T>() != null)
            {
                GameObject instance = Instantiate(prefs[i]);
                //Debug.Log("Created!");
                return instance.GetComponent<T>();
            }
        }
        return default(T);
    }
    
    /*
    public GameObject Get(EntityType type)
    {
        for (int i = 0; i < poolable.Count; i++)
        {
            if (poolable[i].Type == type)
            {
                GameObject go = poolable[i].Get;
                go.SetActive(true);
                poolable.Remove(poolable[i]);
                return go;
            }
        }

        switch (type) {
            case EntityType.ASTEROID:
                return Instantiate(asteroidPrephab);
            case EntityType.SHIP:
                return Instantiate(shipPrephab);
            case EntityType.LASER:
                return Instantiate(laserPrephab);
            case EntityType.DAMAGEPOPUP:
                return Instantiate(damagePopUpPrephab);
        }
        return null;
    }
    */
}