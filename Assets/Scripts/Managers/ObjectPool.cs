using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject pooledObject;
    public int pooledAmount;
    public bool willGrow;

    public List<GameObject> pooledObjects;

    private void Awake()
    {
        instance = this;

        CreatePooledObjectList();
    }

    public void CreatePooledObjectList()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject objectToPool = Instantiate(pooledObject);
            objectToPool.SetActive(false);
            pooledObjects.Add(objectToPool);
        }
    }

    public GameObject GetPooledObject() 
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false)
            { 
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject objectToPool = Instantiate(pooledObject);
            pooledObjects.Add(objectToPool);
            return objectToPool;
        }

        return null;
    }
}
