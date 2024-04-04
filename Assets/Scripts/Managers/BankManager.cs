using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankManager : MonoBehaviour
{
    public static BankManager Instance;
    public int currentMoneyAmount;
    [SerializeField] int initialMoneyAmount;
    [SerializeField] TextMeshProUGUI moneyAmountText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        { 
            Instance = this;
        }
    }

    private void Start()
    {
        currentMoneyAmount = initialMoneyAmount;
        SetUIMoneyAmount();
    }

    public bool CanAfford(int cost)
    {
        return currentMoneyAmount >= cost;
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
