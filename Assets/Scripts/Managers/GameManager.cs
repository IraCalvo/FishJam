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
    public List<GameObject> fishActive;
    public List<GameObject> enemiesActive;

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
            fishActive.Add(gameObject);
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            enemiesActive.Add(gameObject);
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
            fishActive.Remove(gameObject);
            CheckFishList();
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            enemiesActive.Remove(gameObject);
            CheckEnemyList();
        }
        else
        {
            return;
        }
    }

    void CheckFishList()
    {
        if (fishActive.Count <= 0)
        { 
            PlayerLose();
        }
    }

    void CheckEnemyList()
    {
        if (enemiesActive.Count <= 0)
        {
            Fish[] fish = new Fish[fishActive.Count];
            foreach (Fish f in fish)
            {
                f.fishState.SetStateTo(FishState.State.Normal);
            }
        }
    }

    void SwitchFishToCombat()
    {
        Fish[] fish = new Fish[fishActive.Count];
        foreach (Fish f in fish)
        {
            f.fishState.SetStateTo(FishState.State.Combat);
        }
    }

    //TODO: make an actual lose process
    public void PlayerLose()
    {
        Destroy(player);
        loseScreen.SetActive(true);
    }
}
