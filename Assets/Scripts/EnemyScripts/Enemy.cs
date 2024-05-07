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

        foreach (Fish f in GameManager.instance.activeFish)
        {
            f.fishState.SetStateTo(FishState.State.Combat);
        }
    }

    public void TakeDamage(int damageToTake)
    {
        currentHP -= damageToTake;

        if (currentHP <= 0)
        {
            PoolManager.instance.DeactivateObjectInPool(gameObject);
            GameObject damagePopUp = PoolManager.instance.GetPoolObject(PoolObjectType.DamagePopUp);
            damagePopUp.transform.position = transform.position;
            damagePopUp.GetComponent<DamagePopup>().Setup(damageToTake);
        }

        if (EnemyHealthBar.instance.healthBarIsActive)
        { 
            EnemyHealthBar.instance.UpdateHealthBar();
        }
    }
}
