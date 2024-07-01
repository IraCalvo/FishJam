using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Renderer))]
public abstract class Resource : MonoBehaviour 
{
    public ResourceSO resourceSO;
    private bool didClick = false;
    private float timer;
    private float moveSpeed = 50;

    Rigidbody2D rb;
    Renderer objRenderer;

    private void Awake()
    {
        rb =  GetComponent<Rigidbody2D>();
        objRenderer = GetComponent<Renderer>();
    }

    public abstract void AbstractAwake();
    public abstract void AbstractFixedUpdate();

    public virtual void OnDisable()
    {
        ResetResource();
    }

    public virtual void ResourceClicked()
    {
        SFXManager.instance.PlaySFX(SoundType.CoinCollected);
        if (didClick) { return; }
        didClick = true;

        // Reset alpha
        Color objColor = objRenderer.material.color;
        objColor.a = 1f;
        objRenderer.material.color = objColor;

        //move it towards the bank
        StartCoroutine(AnimateResourceCollected());
    }

    IEnumerator AnimateResourceCollected()
    {
        Vector2 topRightCorner = new Vector2(Screen.width, Screen.height);
        Vector3 bankPosition = Camera.main.ScreenToWorldPoint(topRightCorner);

        while (Vector2.Distance(transform.position, bankPosition) > 1f)
        {
            Vector2 direction = bankPosition - transform.position;
            direction.Normalize();
            Vector2 movement = direction * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(movement.x, movement.y, 0);

            yield return null;
        }
        BankManager.Instance.AddMoney(resourceSO.resourceValue);
        PoolManager.instance.DeactivateObjectInPool(gameObject);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            StartCoroutine(FadeAwayCoroutine());
        }
    }

    IEnumerator FadeAwayCoroutine() 
    {
        timer = resourceSO.disappearTime;
        while (timer > 0)
        { 
            float alphaAmount = timer / resourceSO.disappearTime;
            alphaAmount = Mathf.Clamp01(alphaAmount);

            Color objColor = objRenderer.material.color;
            objColor.a = alphaAmount;
            if (didClick) { 
                objColor.a = 1f;
                break; 
            }
            objRenderer.material.color = objColor;

            timer -= Time.deltaTime;

            yield return null;
        }

        if (!didClick)
        {
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }
    }

    private void ResetResource()
    {
        Color objColor = objRenderer.material.color;
        objColor.a = 1f;
        objRenderer.material.color = objColor;

        rb.gravityScale = 1;

        didClick = false;
    }
}
