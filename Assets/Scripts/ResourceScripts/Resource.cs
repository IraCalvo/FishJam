using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Renderer))]
public abstract class Resource : MonoBehaviour 
{
    public ResourceSO resourceSO;

    Rigidbody2D rb;
    Renderer objRenderer;

    private void Awake()
    {
        rb =  GetComponent<Rigidbody2D>();
        objRenderer = GetComponent<Renderer>();
    }

    public abstract void AbstractAwake();
    public abstract void AbstractFixedUpdate();

    public virtual void ResourceClicked()
    {
        BankManager.Instance.AddMoney(resourceSO.resourceValue);
        //move it towards the bank
        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            StartCoroutine(FadeAwayCoroutine());
        }
    }

    IEnumerator FadeAwayCoroutine() 
    {
        float timer = resourceSO.disappearTime;
        while (timer > 0)
        { 
            float alphaAmount = timer / resourceSO.disappearTime;
            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objColor = objRenderer.material.color;
            objColor.a = alphaAmount;
            objRenderer.material.color = objColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }
}
