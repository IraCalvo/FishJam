using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterFish : MonoBehaviour
{
    void Start()
    {
        SetToPool();
    }

    void SetToPool()
    {

        if (TryGetComponent<Fish>(out var starterFish))
        {
            PoolInfo fishPool = PoolManager.instance.GetPoolByFishName(starterFish.fishSO.fishName);
            if (fishPool != null)
            {
                gameObject.transform.parent = fishPool.container.transform;
            }

            GameManager.instance.AddToActiveList(gameObject);
            gameObject.SetActive(true);
            starterFish.fishState.SetStateTo(FishState.State.Normal);

            if (TryGetComponent<FishMovement>(out FishMovement moventScipt))
            {
                moventScipt.PickRandomLocation();
            }
        }
    }
}
