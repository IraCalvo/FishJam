using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSO fishSO;
    public FishSpecies fishSpecies;
    public FishState fishState;
    public bool isOnScreen;

    private int currentHP;
    //List<

    [Header("Misc")]
    SpriteRenderer sr;
    Rigidbody2D rb;
    GameObject tank;
    Bounds tankBounds;

    private void Awake()
    {
        fishState.SetStateTo(FishState.State.Spawning);

        sr = GetComponent<SpriteRenderer>();   
        rb = GetComponent<Rigidbody2D>();
        tank = GameObject.Find("Tank");
        tankBounds= tank.GetComponent<PolygonCollider2D>().bounds;

        currentHP = fishSO.hp;
    }

    private void OnEnable()
    {
        fishState.SetStateTo(FishState.State.Spawning);
        if (FishList.instance != null)
        {
            FishList.instance.UpdateFishList();
        }
    }

    private void OnDestroy()
    {
        FishList.instance.UpdateFishList();
    }

    private void Update()
    {
        if (!isOnScreen)
        {
            sr.enabled = true;
        }
        else 
        {
            sr.enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }
    }

    //private void OnBecameInvisible()
    //{
    //    isOnScreen = false;
    //}

    //private void OnBecameVisible()
    //{
    //    isOnScreen = true;
    //}
}
