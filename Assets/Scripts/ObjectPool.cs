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
    [SerializeField]
    private GameObject damagePopUpPrephab;
    [SerializeField]
    private GameObject asteroidPref;

    private void Awake() {
        allObjcets = new List<IPoolable>();
    }

    public void Add(IPoolable obj)
    {
        allObjcets.Add(obj);
    }

    public GameObject Get(EntityType type)
    {
        for (int i = 0; i < allObjcets.Count; i++)
        {
            if (allObjcets[i].Type == type)
            {
                GameObject go = allObjcets[i].Get;
                go.SetActive(true);
                allObjcets.Remove(allObjcets[i]);
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
}