using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] int amountWorth;
    Rigidbody2D rb;
    Fade fadeScript;

    public override void UseItem(Vector2 spawnPosition)
    {
        if (BankManager.Instance.CanAfford(amountWorth)) {
            BankManager.Instance.RemoveMoney(amountWorth);
            Instantiate(gameObject, spawnPosition, Quaternion.identity);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fadeScript = GetComponent<Fade>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            fadeScript.StartFade();
        }
    }
}
