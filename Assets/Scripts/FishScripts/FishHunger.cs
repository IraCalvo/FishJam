using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHunger : MonoBehaviour
{
    private Fish fish;
    private FishSO fishSO;
    private FishState fishState;
    private float hungerTimer;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
        fishState = fish.fishState;
    }

    // Update is called once per frame
    void Update()
    {
        if (fishState.GetCurrentState() != FishState.State.Spawning)
        {
            HungerTimer();
        }
    }

    void HungerTimer()
    {
        // Already hungry
        if (fishState.GetCurrentState() == FishState.State.Hungry)
        {
            return;
        }

        if (hungerTimer <= 0)
        {
            fishState.SetStateTo(FishState.State.Hungry);
            hungerTimer = fishSO.hungerTimerMax;
        }
        else
        {
            hungerTimer -= Time.deltaTime;
        }
    }
}
