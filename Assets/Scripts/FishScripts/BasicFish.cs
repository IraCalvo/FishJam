using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFish : MonoBehaviour
{
    [SerializeField] float minMoneyTimer;
    [SerializeField] float maxMoneyTimer;
    private float moneyTimer;
    [SerializeField] GameObject moneyToDrop;

    private void Start()
    {
        ChooseMoneyTimer();
    }

    private void Update()
    {
        SpawnMoney();
    }

    void SpawnMoney()
    {
        if (moneyTimer <= 0)
        {
            Instantiate(moneyToDrop);
            ChooseMoneyTimer();
        }
        else
        {
            moneyTimer -= Time.deltaTime;
        }
    }

    void ChooseMoneyTimer()
    {
        moneyTimer = Random.Range(minMoneyTimer, maxMoneyTimer);
    }
}
