using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class BasicFish : MonoBehaviour
{
    [Header("Fish Movement")]
    [SerializeField] float fishMoveSpeed;
    [SerializeField] float minLocationPickTimer;
    [SerializeField] float maxLocationPickTimer;
    public float nextLocationTimer;
    public Vector2 targetPosition;
    bool isMoving;
    bool canMove;
    public bool isSpawning = true;

    [Header("Money Information")]
    [SerializeField] float minMoneyTimer;
    [SerializeField] float maxMoneyTimer;
    private float moneyTimer;
    [SerializeField] GameObject moneyToDrop;

    [Header("Misc")]
    SpriteRenderer sr;
    Rigidbody2D rb;
    GameObject tank;
    Bounds tankBounds;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();   
        rb = GetComponent<Rigidbody2D>();
        tank = GameObject.Find("Tank");
        tankBounds= tank.GetComponent<PolygonCollider2D>().bounds;
    }
    private void Start()
    {
        isSpawning = true;
        ChooseMoneyTimer();
    }

    private void Update()
    {
        SpawnMoney();
    }

    private void FixedUpdate()
    {
        if (isSpawning)
        {
            MoveFishToSpawn();
        }
        else
        {
            MoveFish();
        }
    }
    
    void SpawnMoney()
    {
        if (moneyTimer <= 0)
        {
            Instantiate(moneyToDrop, transform.position, Quaternion.identity);
            ChooseMoneyTimer();
        }
        else
        {
            moneyTimer -= Time.deltaTime;
        }
    }

    void MoveFish()
    {
        if (nextLocationTimer <= 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, fishMoveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                PickWaitTimer();
                PickRandomLocation();
            }
        }
        else
        { 
            nextLocationTimer -= Time.deltaTime;
        }
    }

    void MoveFishToSpawn()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            float t = 1f - Mathf.Clamp01(distanceToTarget / 10); // Clamping to ensure t is between 0 and 1
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Apply easing function
            float spawnMoveSpeed = Mathf.Lerp(fishMoveSpeed * 6, 0f, easedT); // Interpolate movement speed based on eased t
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, spawnMoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            isSpawning = false;
        }
    }

    void PickRandomLocation()
    {
        float randomX = Random.Range(tankBounds.min.x - 3f, tankBounds.max.x - 3f);
        float randomY = Random.Range(tankBounds.min.y - 1f, tankBounds.max.y - 1f);
        
        targetPosition = new Vector2(randomX, randomY);
        SpriteDirection(targetPosition);
    }
    
    void PickWaitTimer()
    {
        nextLocationTimer = Random.Range(minLocationPickTimer, maxLocationPickTimer);
    }

    void ChooseMoneyTimer()
    {
        moneyTimer = Random.Range(minMoneyTimer, maxMoneyTimer);
    }

    void SpriteDirection(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {  
            sr.flipX = false;
        }
    }
}
