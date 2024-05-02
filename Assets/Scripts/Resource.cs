using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int amountWorth;
    [SerializeField] float disappearTime;
    [SerializeField] PoolObjectType resourcePoolObjectType;
    Rigidbody2D rb;
    Renderer objectRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        objectRenderer = GetComponent<Renderer>();
    }
    public void ResourceClicked()
    {
        BankManager.Instance.AddMoney(amountWorth);
        //move coin towards the money later
        PoolManager.instance.DeactivateObjectInPool(this.gameObject, resourcePoolObjectType);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            StartCoroutine(FadeAwayCounter());
        }
    }

    IEnumerator FadeAwayCounter()
    {
        float timer = disappearTime;
        while (timer > 0f)
        {
            float alphaAmount = timer / disappearTime;
            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objectColor = objectRenderer.material.color;
            objectColor.a = alphaAmount;
            objectRenderer.material.color = objectColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(this.gameObject, resourcePoolObjectType);
    }
}
