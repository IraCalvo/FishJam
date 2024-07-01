using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject loseScreen;

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

    //TODO: make an actual lose proccess
    public void PlayerLose()
    {
        Time.timeScale = 0;
        loseScreen.SetActive(true);
    }
}
