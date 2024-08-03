using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        EnemyHealthBar.instance.ShowHealthBar(this);
    }

    public EnemySO enemySO;
    public float currentHP;

    [Header("Defeat Anim Numbers ")]
    SpriteRenderer sr;
    [SerializeField] private float sinkLimit = 4.5f;
    [SerializeField] private float sinkSpeed = 1f;
    [SerializeField] private float fadeSpeed = 0.5f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

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

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    public void TakeDamage(int damageToTake)
    {
        if (currentHP > 0)
        { 
            currentHP -= damageToTake;
            GameObject damagePopUp = PoolManager.instance.GetPoolObject(PoolObjectType.DamagePopUp);
            damagePopUp.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            damagePopUp.GetComponent<DamagePopup>().Setup(damageToTake);
        }

        if (currentHP <= 0)
        {
            StartCoroutine(EnemyDefeatedAnimCoroutine());

            if (EnemyHealthBar.instance.healthBarIsActive)
            { 
                if(EnemyHealthBar.instance.enemyHPToShow.gameObject == this.gameObject)
                {
                    EnemyHealthBar.instance.DeactivateHealthBar();  
                }
            }
        }

        if (EnemyHealthBar.instance.healthBarIsActive)
        { 
            EnemyHealthBar.instance.UpdateHealthBar();
        }
    }

    IEnumerator EnemyDefeatedAnimCoroutine()
    {
        GameObject sandDollar = PoolManager.instance.GetPoolObject(enemySO.sandDollarToDrop);
        sandDollar.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        if (enemySO.moneyToDrop != null)
        {
            GameObject money = PoolManager.instance.GetPoolObject(enemySO.moneyToDrop);
            money.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        float elapsedTime = 0f;
        Color color = sr.color;
        float startYPos = transform.position.y;

        while (transform.position.y > startYPos - sinkLimit)
        {
            transform.Translate(Vector2.down * sinkSpeed * Time.deltaTime, Space.World);
            sr.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }
}
