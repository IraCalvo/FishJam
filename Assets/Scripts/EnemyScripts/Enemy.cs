using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    public float currentHP;
    FishState[] fishInTank;

    private void Awake()
    {
        currentHP = enemySO.MaxHP;

        fishInTank = FindObjectsOfType<FishState>();
        foreach (FishState f in fishInTank)
        { 
            FishState fishState = f.GetComponent<FishState>();
            Fish fish = f.GetComponent<Fish>();
            if (!fish.fishSO.classes.Contains(FishClass.Resource))
            {
                fishState.SetStateTo(FishState.State.Combat);
            }
            else
            {
                return;
            }
        }
    }

    public void TakeDamage(int damageToTake)
    { 
        currentHP -= damageToTake;
        if (EnemyHealthBar.instance.healthBarIsActive)
        { 
            EnemyHealthBar.instance.UpdateHealthBar();
        }
    }
}
