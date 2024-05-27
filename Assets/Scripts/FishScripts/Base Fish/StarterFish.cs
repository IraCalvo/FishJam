using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterFish : MonoBehaviour
{
    private float delayTimer = 1f;

    void Start()
    {
        StartCoroutine(SetToPool());
    }

    IEnumerator SetToPool()
    {
        yield return new WaitForSeconds(delayTimer);

        if (TryGetComponent<Fish>(out var starterFish))
        {
            PoolInfo fishPool = PoolManager.instance.GetPoolByFishName(starterFish.fishSO.fishName);
            if (fishPool != null)
            {
                gameObject.transform.parent = fishPool.container.transform;
            }

            GameManager.instance.AddToActiveList(gameObject);
            gameObject.SetActive(true);
            starterFish.fishState.SetStateTo(FishState.State.Spawning);
        }
    }
}
