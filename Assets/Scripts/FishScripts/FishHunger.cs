using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHunger : MonoBehaviour
{
    private Fish fish;
    private FishSO fishSO;
    private FishState fishState;
    private float hungerTimer;
    private float dieFromHungerTimer;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
        fishState = fish.fishState;
        hungerTimer = fishSO.hungerTimerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (fishState.GetCurrentState() != FishState.State.Spawning && fishState.GetCurrentState() != FishState.State.Dead)
        {
            HungerTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (fishState.GetCurrentState() == FishState.State.Hungry)
        {
            if (collidedObject.TryGetComponent<Food>(out Food food))
            {
                if (fishSO.preferredFoods.Contains(food.foodType))
                {
                    FeedFish(collidedObject);
                }
            }
            else if (collidedObject.TryGetComponent<Fish>(out Fish fish))
            {
                if (fishSO.preferredFoods.Contains(fish.fishSO.foodType))
                {
                    FeedFish(collidedObject);
                }
            }
        }
    }

    private void FeedFish(GameObject gameObject)
    {
        Debug.Log("Feed Fish");
        if (gameObject.TryGetComponent<Food>(out Food food))
        {
            PoolManager.instance.DeactivateObjectInPool(gameObject, food.poolType);
        }
        else if (gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            PoolManager.instance.DeactivateObjectInPool(fish.gameObject, fish.fishSO.fishPoolObjectType);
        }
        
        fishState.SetStateTo(FishState.State.Normal);
    }

    void HungerTimer()
    {
        // Already hungry
        if (fishState.GetCurrentState() == FishState.State.Hungry)
        {
            if (dieFromHungerTimer <= 0)
            {
                fishState.SetStateTo(FishState.State.Dead);
            }
            else
            {
                dieFromHungerTimer -= Time.deltaTime;
            }
            return;
        }

        if (hungerTimer <= 0)
        {
            fishState.SetStateTo(FishState.State.Hungry);
            hungerTimer = fishSO.hungerTimerMax;
            dieFromHungerTimer = fishSO.dieFromHungerTimerMax;
        }
        else
        {
            hungerTimer -= Time.deltaTime;
        }
    }
}
