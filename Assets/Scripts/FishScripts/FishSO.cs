using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/FishSO")]
public class FishSO : ScriptableObject
{
    [Header("General Info")]
    public int price;
    public float moveSpeed;
    public List<FishClass> classes;
    public FoodType foodType;
    public float minLocationPickTimer;
    public float maxLocationPickTimer;

    [Header("Combat Stats")]
    public int damage;
    public int hp;
    public float attackRange;
    public float combatMoveSpeed;
    public float attackCooldown;

    [Header("Food Stats")]
    public List<FoodType> preferredFoods;
    public float foodDetectionRange;
    public float hungerTimerMax;
    public float dieFromHungerTimerMax;

    [Header("Money Stats")]
    public float minMoneyTimer;
    public float maxMoneyTimer;
    public GameObject moneyToDrop;
}
