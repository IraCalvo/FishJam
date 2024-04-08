using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : MonoBehaviour
{

    public int price = 5000;

    public void BuyTrophy()
    {
        if (BankManager.Instance.CanAfford(price))
        {
            BankManager.Instance.RemoveMoney(price);
            Debug.Log("Congrats you won");
        }
    }
}
