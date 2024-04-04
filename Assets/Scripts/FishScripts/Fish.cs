using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Fish : MonoBehaviour
{

    public FishSO fishSO;
    public FishState fishState;

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
    }
    private void Start()
    {
    }

    private void Update()
    {
    }
    
}
