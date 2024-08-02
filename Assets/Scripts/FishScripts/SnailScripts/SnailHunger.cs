using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHunger : MonoBehaviour
{
    private Fish fish;
    private FishSO fishSO;
    private FishState fishState;
    private float dieFromHungerTimer;
    [SerializeField] private int coinsToEatToConvert;
    private int coinsEaten;
    private int pearlWorth;
    public int moneyToTakeAwayFromInitialCollect;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
        fishState = fish.fishState;
        dieFromHungerTimer = fishSO.hungerTimerMax;
    }

    private void Update()
    {
        if (fishState.GetCurrentState() != FishState.State.Spawning && fishState.GetCurrentState() != FishState.State.Dead)
        {
            HungerTimer();
        }
    }

    void HungerTimer()
    {
        if (dieFromHungerTimer <= 0)
        {
            fishState.SetStateTo(FishState.State.Dead);
        }
        else
        {
            dieFromHungerTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Resource>(out Resource resource))
        { 
            if(resource.resourceSO.resourceName != "Pearl" && resource.didClick == false)
            {
                FeedSnail(resource);
            }
        }
    }

    void FeedSnail(Resource resource)
    {
        int initialMoney = resource.resourceSO.resourceValue - moneyToTakeAwayFromInitialCollect;
        BankManager.Instance.AddMoney(initialMoney);
        pearlWorth += resource.resourceSO.resourceValue - initialMoney;
        PoolManager.instance.DeactivateObjectInPool(resource.gameObject);
        dieFromHungerTimer = fishSO.hungerTimerMax;
        coinsEaten++;
        //add eating sfx here
        SFXManager.instance.PlaySFX(SoundType.Slurp);
        if (coinsEaten >= coinsToEatToConvert)
        {
            GameObject pearl = PoolManager.instance.GetPoolObject(fishSO.moneyToDrop);
            pearl.transform.SetPositionAndRotation(gameObject.transform.position, Quaternion.identity);
            pearl.GetComponent<Pearl>().resourceSO.resourceValue = pearlWorth;
            pearlWorth = 0;
            coinsEaten = 0;
        }
    }
}
