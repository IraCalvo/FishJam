using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public static BankManager instance;
    public int currentMoneyAmount;
    [SerializeField] int initialMoneyAmount;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        { 
            instance = this;
        }
    }

    private void Start()
    {
        currentMoneyAmount = initialMoneyAmount;
    }

    public void AddMoney(int moneyToAdd)
    { 
        
    }

    public void RemoveMoney(int moneyToSubtract) 
    {
        currentMoneyAmount -= moneyToSubtract;
    }
}
