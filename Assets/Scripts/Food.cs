using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] int amountWorth;
    [SerializeField] float disappearTime;
    Rigidbody2D rb;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            StartCoroutine(ResourceCountDown());
        }
    }

    IEnumerator ResourceCountDown()
    {
        yield return new WaitForSeconds(disappearTime);
        Destroy(this.gameObject);
    }

}
