using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int fishActiveInTank;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject player;

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

    public void AddFishToActiveList()
    {
        fishActiveInTank++;
    }

    public void RemoveFishFromActiveList()
    { 
        fishActiveInTank--;
        CheckFishList();
    }

    void CheckFishList()
    {
        if (fishActiveInTank <= 0)
        { 
            PlayerLose();
        }
    }

    public void PlayerLose()
    {
        Destroy(player);
        loseScreen.SetActive(true);
    }
}
