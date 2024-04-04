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

    // Start is called before the first frame update
    void Start()
    {
        ChooseMoneyTimer();
    }

    // Update is called once per frame
    void Update()
    {
        SpawnMoney();
    }

    void SpawnMoney()
    {
        if (moneyTimer <= 0)
        {
            Instantiate(fishSO.moneyToDrop, transform.position, Quaternion.identity);
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
