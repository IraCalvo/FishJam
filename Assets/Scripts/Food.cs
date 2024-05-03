using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] int amountWorth;
    Rigidbody2D rb;
    public FoodType foodType;
    public PoolObjectType poolType;

    public float fadeDuration;
    private Renderer objectRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        objectRenderer = GetComponent<Renderer>();
    }

    public override void UseItem(Vector2 spawnPosition)
    {
        if (BankManager.Instance.CanAfford(amountWorth)) 
        {
            BankManager.Instance.RemoveMoney(amountWorth);
            GameObject food = PoolManager.instance.GetPoolObject(poolType);
            food.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            StartCoroutine(FadeAwayCoroutine());
        }
    }

    IEnumerator FadeAwayCoroutine()
    {
        float timer = fadeDuration;
        while (timer > 0f)
        {
            float alphaAmount = timer / fadeDuration;
            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objectColor = objectRenderer.material.color;
            objectColor.a = alphaAmount;
            objectRenderer.material.color = objectColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }
}
