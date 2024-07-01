using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    public float currentHP;

    private void OnEnable()
    {
        currentHP = enemySO.MaxHP;

        EnemySpawnerManager.EnemySpawned();
    }

    private void OnDisable()
    {
        if (GameManager.instance.activeEnemies.Count <= 1)
        {
            EnemySpawnerManager.EnemyDefeated();
        }
    }

    public void TakeDamage(int damageToTake)
    {
        currentHP -= damageToTake;
        GameObject damagePopUp = PoolManager.instance.GetPoolObject(PoolObjectType.DamagePopUp);
        damagePopUp.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        Debug.Log("Enemy Took damage here is Y pos of damage pop up:"  + damagePopUp.transform.position.y);
        damagePopUp.GetComponent<DamagePopup>().Setup(damageToTake);

        if (currentHP <= 0)
        {
            GameObject sandDollar = PoolManager.instance.GetPoolObject(enemySO.sandDollarToDrop);
            sandDollar.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

            if (enemySO.moneyToDrop != null)
            {
                GameObject money = PoolManager.instance.GetPoolObject(enemySO.moneyToDrop);
                money.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }

        if (EnemyHealthBar.instance.healthBarIsActive)
        { 
            EnemyHealthBar.instance.UpdateHealthBar();
        }
    }
}
