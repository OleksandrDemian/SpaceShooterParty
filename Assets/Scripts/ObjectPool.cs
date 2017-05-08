using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefs;
    private List<IPoolable> poolable;

    /// <summary>
    /// Current instance of ObjectPool
    /// </summary>
    public static ObjectPool Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// Called before the game is started
    /// </summary>
    private void Awake() {
        poolable = new List<IPoolable>();
        Instance = this;
    }

    /// <summary>
    /// Add to pool
    /// </summary>
    /// <param name="poolable">Poolable object that must be added</param>
    public void Add(IPoolable poolable)
    {
        this.poolable.Add(poolable);
    }

    /// <summary>
    /// It is used for getting objects from pool
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    /// <returns>Returns the requested object</returns>
    public T Get<T>()
    {
        //Searchs if ialready has the object
        for (int i = 0; i < poolable.Count; i++)
        {
            if(poolable[i] is T)
            {
                IPoolable go = poolable[i];
                go.GetGameObject.SetActive(true);
                poolable.Remove(go);
                return (T)go;
            }
        }

        //Search the object in prefabricated objects
        for (int i = 0; i < prefs.Length; i++)
        {
            //Returns object
            if (prefs[i].GetComponent<T>() != null)
            {
                GameObject instance = Instantiate(prefs[i]);
                return instance.GetComponent<T>();
            }
        }
        //If it has not the object, throws exception
        throw new System.Exception("There is no " + typeof(T));
    }
}