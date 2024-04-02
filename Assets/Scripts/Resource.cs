using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int amountWorth;

    public void ResourceClicked()
    {
        BankManager.instance.AddMoney(amountWorth);
        //move coin towards the money later
        Destroy(gameObject);
    }
}
