using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int amountWorth;
    [SerializeField] float disappearTime;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void ResourceClicked()
    {
        BankManager.Instance.AddMoney(amountWorth);
        //move coin towards the money later
        Destroy(gameObject);
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
