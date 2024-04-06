using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    private Fish crab;
    private FishSO crabSO;
    private FishState crabState;

    public Vector2 targetPosition;
    private float nextLocationTimer;

    public float foodMoveSpeedEase;
    public float jumpForce;

    private bool didDie;
    private Vector2 deathPosition;

    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject tank;
    Bounds tankBounds;
    Animator animator;

    private void Awake()
    {
        crab = GetComponent<Fish>();
        crabSO = crab.fishSO;
        crabState = crab.fishState;
        crabState = GetComponent<FishState>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        switch(crabState.GetCurrentState()) 
        {
            case FishState.State.Normal:
                MoveCrab();
                break;
            case FishState.State.Spawning:
                break;
            case FishState.State.Hungry:
                MoveCrabToFood();
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

    void MoveCrab()
    {
        if (nextLocationTimer <= 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceToTarget = Vector2.Distance(targetPosition, targetPosition);
                //float t = 1f - Mathf.Clamp01(distanceToTarget / 1);
                //float easedT = Mathf.SmoothStep(0f, 1f, t);
                //float easedMoveSpeed = Mathf.Lerp(crabSO.moveSpeed, 0f, easedT);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, crabSO.moveSpeed * Time.fixedDeltaTime);
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

    void MoveCrabToSpawn()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            //float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            //float t = 1f - Mathf.Clamp01(distanceToTarget / 10); // Clamping to ensure t is between 0 and 1
            //float easedT = Mathf.SmoothStep(0f, 1f, t); // Apply easing function
            //float spawnMoveSpeed = Mathf.Lerp(crabSO.moveSpeed * 6, 0f, easedT); // Interpolate movement speed based on eased t
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, crabSO.moveSpeed * Time.fixedDeltaTime);
        }
        else 
        {
            crabState.SetStateTo(FishState.State.Normal);
            PickRandomLocation();
        }
    }

    void MoveCrabToFood()
    {
        Fish[] fish = FindObjectsOfType<Fish>();
        List<Fish> preferredFood = new List<Fish>();
        foreach (Fish f in fish)
        {
            if (f.fishSpecies == FishSpecies.GoldFish && f.fishState.GetCurrentState() != FishState.State.Dead)
            { 
                preferredFood.Add(f);
            }
        }

        Fish closestFish = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Fish f in preferredFood)
        {
            float distance = Vector3.Distance(currentPosition, f.transform.position);
            if (distance < crabSO.foodDetectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestFish = f;
            }
        }

        if (closestFish == null)
        {
            MoveCrab();
            return;
        }

        targetPosition.x = closestFish.transform.position.x;

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float t = 1f - Mathf.Clamp01(distanceToTarget / foodMoveSpeedEase);
        float easedT = Mathf.SmoothStep(0f, 1f, t);
        float easedCrabHungerMoveSpeed = Mathf.Lerp(crabSO.moveSpeed, 0f, easedT);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedCrabHungerMoveSpeed);

        if (Mathf.Abs(transform.position.x - targetPosition.x) < 1f)
        {
            JumpAtFood();
        }
    }

    void JumpAtFood()
    {
        rb.gravityScale = 1f;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoldFish" && crabState.GetCurrentState() == FishState.State.Hungry)
        {
            Destroy(collision.gameObject);
            crabState.SetStateTo(FishState.State.Normal);
            PickRandomLocation();
        }
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            PickRandomLocation();
            crabState.SetStateTo(FishState.State.Normal);
        }
    }

    void MoveFishToDeath()
    {
        animator.enabled = false;
        sr.flipY = true;

        float distanceToTarget = Vector2.Distance(transform.position, deathPosition);
        if (distanceToTarget <= 0.01f)
        {
            Destroy(gameObject);
        }
        float t = 1f - Mathf.Clamp01(distanceToTarget / 2); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(0.5f, 1f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(crabSO.moveSpeed, 0f, easedT); // Interpolate movement speed based on eased t
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
        nextLocationTimer = Random.Range(crabSO.minLocationPickTimer, crabSO.maxLocationPickTimer);
    }

    void PickRandomLocation()
    {
        float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        targetPosition = new Vector2(randomX, transform.position.y);
    }

    void SpriteDirection()
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
