using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    public float currentHP;

    private void Awake()
    {
        currentHP = enemySO.MaxHP;

        FishState[] fishInTank = FindObjectsOfType<FishState>();
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

        if (currentHP <= 0)
        {
            //checks if this the last enemy in the tank
            Enemy[] otherEnemiesInTank = FindObjectsOfType<Enemy>();
            if (otherEnemiesInTank.Length == 1)
            {
                FishState[] fishInTank = FindObjectsOfType<FishState>();
                foreach (FishState f in fishInTank)
                {
                    FishState fishState = f.GetComponent<FishState>();
                    fishState.SetStateTo(FishState.State.Normal);
                }
            }
            PoolManager.instance.DeactivateObjectInPool(this.gameObject, enemySO.enemyPoolObjectType);
        }

        if (EnemyHealthBar.instance.healthBarIsActive)
        { 
            EnemyHealthBar.instance.UpdateHealthBar();
        }
    }
}
