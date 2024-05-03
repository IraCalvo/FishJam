using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{

    public int price = 5000;
    [SerializeField] GameObject winScreen;

    public void BuyTrophy()
    {
        if (BankManager.Instance.CanAfford(price))
        {
            BankManager.Instance.RemoveMoney(price);
            Debug.Log("Congrats you won");
            winScreen.SetActive(true);
        }
    }
}
