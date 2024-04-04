using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FishSO")]
public class FishSO : ScriptableObject
{
    public float moveSpeed;
    public float minLocationPickTimer;
    public float maxLocationPickTimer;
    public float foodDetectionRange;
    public float hungerTimerMax;
    public float minMoneyTimer;
    public float maxMoneyTimer;
    public GameObject moneyToDrop;

    public List<FishClass> classes;
}
