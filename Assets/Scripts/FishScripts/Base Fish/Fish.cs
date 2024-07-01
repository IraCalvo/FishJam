using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Fish : MonoBehaviour
{
    public FishSO fishSO;
    public FishSpecies fishSpecies;
    public FishState fishState;
    public bool isOnScreen;

    private int currentHP;

    [Header("Misc")]
    SpriteRenderer sr;
    Rigidbody2D rb;
    GameObject tank;
    Bounds tankBounds;

    private void Awake()
    {
        if (TryGetComponent<StarterFish>(out StarterFish starterFish))
        {
            return;
        }
        else
        { 
            fishState.SetStateTo(FishState.State.Spawning);    
        }

        sr = GetComponent<SpriteRenderer>();   
        rb = GetComponent<Rigidbody2D>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;

        currentHP = fishSO.hp;
    }

    private void OnEnable()
    {
        if (TryGetComponent<StarterFish>(out StarterFish starterFish))
        {
            return;
        }
        else
        {
            fishState.SetStateTo(FishState.State.Spawning);
        }

        if (FishList.instance != null)
        {
            FishList.instance.UpdateFishList();
        }

        sr.flipY = false;
        EnemySpawnerManager.OnEnemySpawned += EnterCombatState;
        EnemySpawnerManager.OnEnemyDefeated += LeaveCombatState;
    }

    private void OnDisable()
    {
        if (TryGetComponent<StarterFish>(out StarterFish starterFish))
        {
            Destroy(starterFish);
        }
        EnemySpawnerManager.OnEnemySpawned -= EnterCombatState;
        EnemySpawnerManager.OnEnemyDefeated -= LeaveCombatState;
    }

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            PoolManager.instance.GetPoolObject(PoolObjectType.BloodFX);
            FishList.instance.UpdateFishList();
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }
    }

    void EnterCombatState()
    {
        if (gameObject.activeSelf != false)
        {
            fishState.SetStateTo(FishState.State.Combat);
        }
    }

    void LeaveCombatState()
    {
        if (gameObject.activeSelf != false)
        {
            if (TryGetComponent<FishHunger>(out FishHunger fish))
            {
                if (fish.hungerTimer <= 0)
                {
                    fishState.SetStateTo(FishState.State.Hungry);
                }
                else
                {
                    fishState.SetStateTo(FishState.State.Normal);
                }
            }
        }
    }
}
