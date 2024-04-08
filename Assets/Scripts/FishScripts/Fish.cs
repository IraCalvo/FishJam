using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishSO fishSO;
    public FishSpecies fishSpecies;
    public FishState fishState;
    public bool isOnScreen;

    private int currentHP;

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

    private void Update()
    {
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }

    private void OnBecameInvisible()
    {
        isOnScreen = false;
    }

    private void OnBecameVisible()
    {
        isOnScreen = true;
    }
}
