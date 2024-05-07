using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject player;

    public List<Fish> activeFish;
    public List<Enemy> activeEnemies;

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

    public void AddToActiveList(GameObject gameObject)
    {
        if (gameObject.GetComponent<Fish>())
        {
            Fish fish = gameObject.GetComponent<Fish>();
            activeFish.Add(fish);
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            activeEnemies.Add(enemy);
            SwitchFishToCombat();
        }
        else 
        {
            return;
        }
    }

    public void RemoveFromActiveList(GameObject gameObject)
    {
        if (gameObject.GetComponent<Fish>())
        {
            Fish fish = gameObject.GetComponent<Fish>();
            activeFish.Remove(fish);
            CheckFishList();
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            activeEnemies.Remove(enemy);
            CheckEnemyList();
        }
        else
        {
            return;
        }
    }

    void CheckFishList()
    {
        if (activeFish.Count <= 0)
        { 
            PlayerLose();
        }
    }

    void CheckEnemyList()
    {
        if (activeEnemies.Count <= 0)
        {
            foreach (Fish f in activeFish)
            {
                f.fishState.SetStateTo(FishState.State.Normal);
            }
        }
    }

    void SwitchFishToCombat()
    {
        foreach (Fish f in activeFish)
        {
            f.fishState.SetStateTo(FishState.State.Combat);
        }
    }

    //TODO: make an actual lose proccess
    public void PlayerLose()
    {
        Destroy(player);
        loseScreen.SetActive(true);
    }
}
