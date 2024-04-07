using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private Fish fish;
    private FishSO fishSO;
    private FishState fishState;

    public Vector2 targetPosition;
    private float nextLocationTimer;

    SpriteRenderer sr;
    GameObject tank;
    Bounds tankBounds;

    Animator animator;
    private bool didDie;
    private Vector2 deathPosition;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
        fishState = fish.fishState;

        sr = GetComponent<SpriteRenderer>();
        fishState = GetComponent<FishState>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;

        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        switch (fishState.GetCurrentState())
        {
            case FishState.State.Normal:
                MoveFish();
                break;
            case FishState.State.Spawning:
                MoveFishToSpawn();
                break;
            case FishState.State.Hungry:
                MoveFishToFood();
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

    void MoveFish()
    {
        if (nextLocationTimer <= 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
                float t = 1f - Mathf.Clamp01(distanceToTarget / 10); // Clamping to ensure t is between 0 and 1
                float easedT = Mathf.SmoothStep(0.5f, 1f, t); // Apply easing function
                float easedMoveSpeed = Mathf.Lerp(fishSO.moveSpeed, 0f, easedT); // Interpolate movement speed based on eased t
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedMoveSpeed * Time.fixedDeltaTime);
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
            float t = 1f - Mathf.Clamp01(distanceToTarget / 5); // Clamping to ensure t is between 0 and 1
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Apply easing function
            float spawnMoveSpeed = Mathf.Lerp(fishSO.moveSpeed * 7, 0.5f, easedT); // Interpolate movement speed based on eased t
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, spawnMoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Debug.Log("entered early");
            Enemy[] enemiesInTank = FindObjectsOfType<Enemy>();
            if (enemiesInTank.Length == 0 || fishSO.classes.Contains(FishClass.Resource))
            {
                fishState.SetStateTo(FishState.State.Normal);
                PickRandomLocation();
            }
            else if (enemiesInTank != null && !fishSO.classes.Contains(FishClass.Resource))
            {
                fishState.SetStateTo(FishState.State.Combat);
            }
        }
    }

    void MoveFishToFood()
    {
        Food[] foods = FindObjectsOfType<Food>();
        Food closestFood = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Food food in foods)
        {
            float distance = Vector3.Distance(currentPosition, food.transform.position);
            if (distance < fishSO.foodDetectionRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = food;
                }
            }
        }

        // Exit here if no food found, just keep swimming to wherever it was going
        if (closestFood == null)
        {
            MoveFish();
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, closestFood.transform.position);
        float t = 1f - Mathf.Clamp01(distanceToTarget / 5); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(1f, 0f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(fishSO.moveSpeed, 0f, easedT); // Interpolate movement speed based on eased t
        transform.position = Vector2.MoveTowards(transform.position, closestFood.transform.position, easedFishHungerMoveSpeed * Time.fixedDeltaTime);
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
        float t = 1f - Mathf.Clamp01(distanceToTarget / 3); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(0.5f, 1f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(fishSO.moveSpeed, 1f, easedT); // Interpolate movement speed based on eased t
        transform.position = Vector2.MoveTowards(transform.position, deathPosition, easedFishHungerMoveSpeed * Time.fixedDeltaTime);
        
        sr.material.SetFloat("_DeadTimer", Mathf.Clamp(distanceToTarget, 0, 1));
    }

    void SetDeathPosition()
    {
        didDie = true;
        deathPosition = new Vector2(transform.position.x, transform.position.y - 5);
    }

    void PickWaitTimer()
    {
        nextLocationTimer = Random.Range(fishSO.minLocationPickTimer, fishSO.maxLocationPickTimer);
    }

    void PickRandomLocation()
    {
        float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        float randomY = Random.Range(tankBounds.min.y + 1f, tankBounds.max.y - 1f);

        targetPosition = new Vector2(randomX, randomY);
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
