using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishState : MonoBehaviour
{
    public enum State
    {
        Normal,
        Spawning,
        Hungry,
        Combat
    }

    [SerializeField] private State currentState;

    public FishState.State GetCurrentState() { return currentState; }

    public void SetStateTo(FishState.State state)
    {
        currentState = state;
    }
}
