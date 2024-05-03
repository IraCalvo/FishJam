using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{ 
    Food,
    Resource,
    DamagePopUp,
    Fish,
    Enemy
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amountToPool;
    public GameObject prefabToPool;
    public GameObject container;
    public bool willGrow;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [SerializeField]
    public List<PoolInfo> listOfPool;
    private Vector3 defaultPos = new Vector3(0, 0, 0);

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            FillPool(listOfPool[i]);
        }
    }

    void FillPool(PoolInfo info)
    {
        for (int i = 0; i < info.amountToPool; i++)
        {
            GameObject objInstance;
            objInstance = Instantiate(info.prefabToPool, info.container.transform);
            objInstance.SetActive(false);
            objInstance.transform.position = defaultPos;
            info.pool.Add(objInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;
        List<GameObject> nonActiveObjectsInPool = new();
        GameObject objInstance = null;

        for (int i = 0; i < pool.Count; i++)
        {
            //check if game object is active
            if (pool[i].activeInHierarchy == false)
            {
                nonActiveObjectsInPool.Add(pool[i]);
            }
        }

        if (nonActiveObjectsInPool.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, nonActiveObjectsInPool.Count);
            objInstance = nonActiveObjectsInPool[randomIndex];
            objInstance.SetActive(true);
        }
        else if (nonActiveObjectsInPool.Count == 0 && selected.willGrow)
        {
            objInstance = Instantiate(selected.prefabToPool, selected.container.transform);
            objInstance.SetActive(true);
        }

        return objInstance;
    }

    public GameObject GetPoolObject(GameObject gameObject)
    {
        PoolInfo selected = null;
        GameObject objInstance = null;

        if (gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            selected = GetPoolByFishName(fish.fishSO.fishName);
        }
        else if (gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            selected = GetPoolByEnemyName(enemy.enemySO.enemyName);
        }
        else if (gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            selected = GetPoolByResourceName(resource.resourceSO.resourceName);
        }

        List<GameObject> pool = selected.pool;
        List<GameObject> nonActiveObjectsInPool = new();

        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].activeInHierarchy == false)
            {
                nonActiveObjectsInPool.Add(pool[i]);
            }
        }

        if (nonActiveObjectsInPool.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, nonActiveObjectsInPool.Count);
            objInstance = nonActiveObjectsInPool[randomIndex];
            GameManager.instance.AddToActiveList(objInstance);
            objInstance.SetActive(true);
        }
        else if (nonActiveObjectsInPool.Count == 0 && selected.willGrow)
        {
            objInstance = Instantiate(selected.prefabToPool, selected.container.transform);
            objInstance.SetActive(true);
        }

        return objInstance;
    }

    //work on name of function, puts object back into the pool
    public void DeactivateObjectInPool(GameObject obj)
    {
        obj.SetActive(false);
        GameManager.instance.RemoveFromActiveList(obj);
        obj.transform.position = defaultPos;
    }

    //

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            if (type == listOfPool[i].type)
            {
                return listOfPool[i];
            }
        }

        return null;
    }

    private PoolInfo GetPoolByFishName(string fishName)
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.type == PoolObjectType.Fish)
            { 
                Fish fishComponent = poolInfo.prefabToPool.GetComponent<Fish>();
                if (fishComponent.fishSO.fishName == fishName)
                {
                    return poolInfo;
                }
            }
        }
        return null;
    }

    private PoolInfo GetPoolByEnemyName(string enemyName) 
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.type == PoolObjectType.Fish)
            {
                Enemy enemyComponent = poolInfo.prefabToPool.GetComponent<Enemy>();
                if (enemyComponent.enemySO.enemyName == enemyName)
                {
                    return poolInfo;
                }
            }
        }
        return null;
    }

    private PoolInfo GetPoolByResourceName(string resourceName)
    {
        foreach (PoolInfo poolInfo in listOfPool)
        {
            if (poolInfo.type == PoolObjectType.Resource)
            { 
                Resource resourceComponent = poolInfo.prefabToPool.GetComponent<Resource>();
                if (resourceComponent.resourceSO.resourceName == resourceName)
                {
                    return poolInfo;
                }
            }
        }
        return null;
    }
}
