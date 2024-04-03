using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public static BankManager instance;
    public int currentMoneyAmount;
    [SerializeField] int initialMoneyAmount;
    [SerializeField] TextMeshProUGUI moneyAmountText;

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
        SetUIMoneyAmount();
    }

    public void AddMoney(int moneyToAdd)
    { 
        currentMoneyAmount += moneyToAdd;
        SetUIMoneyAmount();
    }

    public void RemoveMoney(int moneyToSubtract) 
    {
        currentMoneyAmount -= moneyToSubtract;
        SetUIMoneyAmount();
    }

    private void SetUIMoneyAmount()
    {
        moneyAmountText.text = currentMoneyAmount.ToString();
    }
}
