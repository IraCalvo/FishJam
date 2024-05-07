using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterFish : MonoBehaviour
{
    void Start()
    { 
        Fish starterFish = GetComponent<Fish>();
        if (starterFish != null) 
        {
            PoolInfo fishPool = PoolManager.instance.GetPoolByFishName(starterFish.fishSO.fishName);
            fishPool.pool.Add(gameObject);
            GameManager.instance.AddToActiveList(gameObject);
        }
    }
}
