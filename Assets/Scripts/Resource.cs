using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int amountWorth;
    [SerializeField] float disappearTime;

    public void ResourceClicked()
    {
        BankManager.instance.AddMoney(amountWorth);
        //move coin towards the money later
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            StartCoroutine(resourceCountDown());
        }   
    }

    IEnumerator resourceCountDown()
    { 
        yield return new WaitForSeconds(disappearTime);
        Destroy(this.gameObject);
    }
}
