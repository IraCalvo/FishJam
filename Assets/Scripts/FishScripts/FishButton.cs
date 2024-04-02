using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.InputSystem;

public class FishButton : MonoBehaviour
{
    [SerializeField] GameObject fishPrefab;
    [SerializeField] int fishCost;

    public void ButtonClicked()
    {
        if (BankManager.instance.currentMoneyAmount >= fishCost)
        {
            BankManager.instance.RemoveMoney(fishCost);
            Instantiate(fishPrefab);
        }
    }
}
