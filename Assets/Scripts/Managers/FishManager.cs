using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField] Fish[] arrayOfFish;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject player;
    public void Update()
    {
        arrayOfFish = FindObjectsOfType<Fish>();
        if (arrayOfFish.Length <= 0)
        {
            //lazy way to disable player for now
            Destroy(player);
            loseScreen.SetActive(true);
        }
    }
}
