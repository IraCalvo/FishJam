using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMoney : MonoBehaviour
{
    private Fish fish;
    private FishSO fishSO;
    private float moneyTimer;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
    }

    void Start()
    {
        ChooseMoneyTimer();
    }

    void Update()
    {
        if (fishSO.moneyToDrop != null)
        {
            SpawnMoney();
        }
        else
        {
            return;
        }
    }

    void SpawnMoney()
    {
        if (moneyTimer <= 0)
        {
            GameObject resource = PoolManager.instance.GetPoolObject(fishSO.moneyToDrop);
            resource.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            ChooseMoneyTimer();
        }
        else
        {
            moneyTimer -= Time.deltaTime;
        }
    }

    void ChooseMoneyTimer()
    {
        moneyTimer = Random.Range(fishSO.minMoneyTimer, fishSO.maxMoneyTimer);
    }

}
