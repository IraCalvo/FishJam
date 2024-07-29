using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnailMovement : MonoBehaviour
{
    private Fish snail;
    private FishSO snailSO;
    private FishState snailState;

    [SerializeField] private Vector2 targetPosition;
    private float nextLocationTimer;

    private bool didDie;
    private Vector2 deathPosition;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    GameObject tank;
    Bounds tankBounds;
    Material material;

    private void Awake()
    {
        snail = GetComponent<Fish>();
        snailSO = snail.fishSO;
        snailState = snail.fishState;
        snailState = GetComponent<FishState>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
    }

    private void FixedUpdate()
    {
        switch (snailState.GetCurrentState())
        {
            case FishState.State.Normal:
                MoveSnail();
                break;
            case FishState.State.Spawning:
                break;
            case FishState.State.Hungry:
                MoveSnailToFood();
                break;
            case FishState.State.Combat:
                break;
            case FishState.State.Dead:
                if (!didDie)
                {
                    SetDeathPosition();
                }
                MoveFishToDeath();
                break;
        }
        SpriteDirection();
    }

    void MoveSnail()
    {
        if (nextLocationTimer <= 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceToTarget = Vector2.Distance(targetPosition, targetPosition);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, snailSO.moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                PickWaitTimer();
                PickRandomLocation();
            }
        }
        else
        {
            snailState.SetStateTo(FishState.State.Normal);
            PickRandomLocation();
        }
    }

    void MoveSnailToFood()
    {
        List<GameObject> preferredFood = new List<GameObject>();

        PoolManager poolManager = PoolManager.instance;
        foreach (FoodType preferredFoodType in snailSO.preferredFoods)
        {
            string foodToResourceName = preferredFoodType.ToString();
            PoolInfo pool = poolManager.GetPoolByResourceName(foodToResourceName);
            //foreach (GameObject go in pool.pool)
            //{
            //    if (go.activeInHierarchy)
            //    {
            //        preferredFood.Add(go);
            //    }
            //}
        }

        if (preferredFood.Count == 0)
        {
            MoveSnail();
            return;
        }

        GameObject closestFood = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject r in preferredFood)
        { 
            float distance = Vector3.Distance(currentPosition, r.transform.position);
            if (distance < snailSO.foodDetectionRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = r;
                }
            }
        }

        if (closestFood == null)
        {
            MoveSnail();
            return;
        }

        targetPosition.x = closestFood.transform.position.x;

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float t = 1f - Mathf.Clamp01(distanceToTarget / snailSO.moveSpeed);
        float easedT = Mathf.SmoothStep(0f, 1f, t);
        float easedSnailMS = Mathf.Lerp(snailSO.moveSpeed, 0f, easedT);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedSnailMS);
    }

    void MoveFishToDeath()
    {
        anim.enabled = false;
        sr.flipY = true;

        float distanceToTarget = Vector2.Distance(transform.position, deathPosition);
        if (distanceToTarget <= 0.01f)
        {
            Destroy(gameObject);
        }
        float t = 1f - Mathf.Clamp01(distanceToTarget / 1); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(0.5f, 1f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(snailSO.moveSpeed, 1f, easedT); // Interpolate movement speed based on eased t
        transform.position = Vector2.MoveTowards(transform.position, deathPosition, easedFishHungerMoveSpeed * Time.fixedDeltaTime);

        sr.material.SetFloat("_DeadTimer", Mathf.Clamp(distanceToTarget, 0, 1));
    }

    void SetDeathPosition()
    {
        didDie = true;
        deathPosition = new Vector2(transform.position.x, transform.position.y - 1);
    }

    void PickWaitTimer()
    {
        nextLocationTimer = Random.Range(snailSO.minLocationPickTimer, snailSO.maxLocationPickTimer);
    }

    void PickRandomLocation()
    {
        float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        targetPosition = new Vector2(randomX, transform.position.y);
    }

    void SpriteDirection()
    {
        if (targetPosition.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            PickRandomLocation();
            snailState.SetStateTo(FishState.State.Hungry);
        }
        if (collision.gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource.resourceSO.resourceName != "Pearl" && resource.resourceSO.resourceName != "Sand Dollar")
            {
                PickRandomLocation();
            }
        }
    }
}
