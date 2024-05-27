using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class FishMovement : MonoBehaviour
{
    public Fish fish;
    public FishSO fishSO;
    public FishState fishState;

    public Vector2 targetPosition;
    public float nextLocationTimer;

    SpriteRenderer sr;
    GameObject tank;
    Bounds tankBounds;
    GameObject ground;
    Bounds groundBounds;

    Animator animator;
    private bool didDie;

    private void Awake()
    {
        fish = GetComponent<Fish>();
        fishSO = fish.fishSO;
        fishState = fish.fishState;

        sr = GetComponent<SpriteRenderer>();
        fishState = GetComponent<FishState>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
        ground = GameObject.Find("Ground");
        groundBounds = ground.GetComponent<BoxCollider2D>().bounds;

        animator = GetComponent<Animator>();

        AbstractAwake();
    }

    public abstract void AbstractAwake();

    public abstract void AbstractFixedUpdate();

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
        AbstractFixedUpdate();
    }

    public virtual void MoveFish()
    {
        if (nextLocationTimer <= 0)
        {
            if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
                float t = 1f - Mathf.Clamp01(distanceToTarget / 4); // Clamping to ensure t is between 0 and 1
                float easedT = Mathf.SmoothStep(0.5f, 0.5f, t); // Apply easing function
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

    public virtual void MoveFishToSpawn()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            float t = 1f - Mathf.Clamp01(distanceToTarget / 5);
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            float spawnMoveSpeed = Mathf.Lerp(fishSO.moveSpeed * 7, 0.5f, easedT);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, spawnMoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (GameManager.instance.activeEnemies.Count == 0 || fishSO.classes.Contains(FishClass.Resource))
            {
                fishState.SetStateTo(FishState.State.Normal);
                PickRandomLocation();
            }
            else if (GameManager.instance.activeEnemies.Count > 0 && !fishSO.classes.Contains(FishClass.Resource))
            {
                fishState.SetStateTo(FishState.State.Combat);
            }
        }
    }

    public virtual void MoveFishToFood()
    {

        List<GameObject> preferredFood = new List<GameObject>();

        PoolManager poolManager = PoolManager.instance;
        foreach (FoodType preferredFoodType in fishSO.preferredFoods)
        {
            PoolInfo pool = poolManager.GetPoolByFoodType(preferredFoodType);
            foreach (GameObject go in pool.pool)
            {
                if (go.activeInHierarchy)
                {
                    preferredFood.Add(go);
                }
            }
        }

        if (preferredFood.Count == 0)
        {
            MoveFish();
            return;
        }

        GameObject closestFood = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject f in preferredFood)
        {
            float distance = Vector3.Distance(currentPosition, f.transform.position);
            if (distance < fishSO.foodDetectionRange)
            {
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFood = f;
                }
            }
        }

        // Exit here if no food found, just keep swimming to wherever it was going
        if (closestFood == null)
        {
            MoveFish();
            return;
        }

        targetPosition = closestFood.transform.position;

        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        float t = 1f - Mathf.Clamp01(distanceToTarget / 5); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(0.5f, 0.5f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(fishSO.moveSpeed, 0f, easedT); // Interpolate movement speed based on eased t
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedFishHungerMoveSpeed * Time.fixedDeltaTime);
    }

    public virtual void MoveFishToDeath()
    {
        animator.enabled = false;
        sr.flipY = true;
        
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget <= 0.1f)
        {
            PoolManager.instance.DeactivateObjectInPool(gameObject);
        }
        float t = 1f - Mathf.Clamp01(distanceToTarget / 3); // Clamping to ensure t is between 0 and 1
        float easedT = Mathf.SmoothStep(0.5f, 0.5f, t); // Apply easing function
        float easedFishHungerMoveSpeed = Mathf.Lerp(fishSO.moveSpeed, 0.2f, easedT); // Interpolate movement speed based on eased t
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedFishHungerMoveSpeed * Time.fixedDeltaTime);

        float deadTimerFloat = Mathf.Clamp01(distanceToTarget / 5) - 0.1f;
        sr.material.SetFloat("_DeadTimer", deadTimerFloat);
        if (TryGetComponent<FishMaterialManager>(out  FishMaterialManager manager))
        {
            manager.deadMaterial.SetFloat("_DeadTimer", deadTimerFloat);
        }
    }

    private void SetDeathPosition()
    {
        didDie = true;
        targetPosition = new Vector2(transform.position.x, transform.position.y - 5);
    }

    void PickWaitTimer()
    {
        nextLocationTimer = Random.Range(fishSO.minLocationPickTimer, fishSO.maxLocationPickTimer);
    }

    public virtual void PickRandomLocation()
    {
        float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        float randomY = Random.Range(groundBounds.max.y + 1f, tankBounds.max.y - 1f);

        targetPosition = new Vector2(randomX, randomY);
    }

    private void SpriteDirection()
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
