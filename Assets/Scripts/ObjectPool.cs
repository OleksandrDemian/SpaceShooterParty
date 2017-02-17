using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<IPoolable> allObjcets;

    [SerializeField]
    private GameObject shipPrephab;
    [SerializeField]
    private GameObject asteroidPrephab;
    [SerializeField]
    private GameObject laserPrephab;

    private void Awake() {
        allObjcets = new List<IPoolable>();
    }

    public void Add(IPoolable obj) {
        allObjcets.Add(obj);
    }

    public GameObject Get(GOType type) {
        for (int i = 0; i < allObjcets.Count; i++)
        {
            if (allObjcets[i].Type == type)
            {
                GameObject go = allObjcets[i].Get;
                go.SetActive(true);
                Debug.Log("Creo: " + allObjcets[i].Type + " instead of " + type);
                allObjcets.Remove(allObjcets[i]);
                return go;
            }
        }

        switch (type) {
            case GOType.ASTEROID:
                return Instantiate(asteroidPrephab);
            case GOType.SHIP:
                return Instantiate(shipPrephab);
            case GOType.LASER:
                return Instantiate(laserPrephab);
        }
        return null;
    }
}