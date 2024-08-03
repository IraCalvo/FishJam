using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChargeDiverMovement : MonoBehaviour
{
    private enum State
    {
        Idle,
        Aiming,
        Rotating,
        Charging,
        Attacking,
        Resetting,
        Dead
    }

    [SerializeField] State state;
    private Enemy enemy;
    private EnemySO enemySO;
    public Vector2 targetPosition;
    public float rotationSpeed;

    SpriteRenderer spriteRenderer;
    public float chargeTimer;
    GameObject tank;
    Bounds tankBounds;
    Animator animator;

    public float minChargeDistance;
    public float maxChargeDistance;
    [SerializeField] private bool didAim;
    private bool didCharge;

    public GameObject targetSprite;
    private GameObject target;
    public List<GameObject> targets;

    public HashSet<GameObject> alreadyHitTargets = new HashSet<GameObject>();

    private void Awake()
    {
        state = State.Idle;
        enemy = GetComponent<Enemy>();
        enemySO = enemy.enemySO;
        spriteRenderer = GetComponent<SpriteRenderer>();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                PickRandomLocation();
                break;
            case State.Aiming:
                if (!didAim)
                {
                    RotateSprite();
                }
                break;
            case State.Charging:
                if (!didCharge)
                {
                    StartCoroutine(Charge());
                }
                break;
            case State.Attacking:
                Move();
                break;
            case State.Resetting:
                Reset();
                break;
            case State.Dead:
                Reset();
                break;
        }

        CheckDeath();
    }

    void Move()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            //float t = 1f - Mathf.Clamp01(distanceToTarget / 3); // Clamping to ensure t is between 0 and 1
            //float easedT = Mathf.SmoothStep(0.5f, 0.5f, t); // Apply easing function
            //float easedMoveSpeed = Mathf.Lerp(enemySO.speed, 0.5f, easedT); // Interpolate movement speed based on eased t

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySO.speed * Time.fixedDeltaTime);
            Destroy(target);
        }
        else
        {
            state = State.Resetting;
            alreadyHitTargets.Clear();
        }
    }

    void RotateSprite()
    {
        didAim = true;
        // Calculate the direction vector from the object to the target
        Vector2 direction = targetPosition - (Vector2)transform.position;

        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a Quaternion rotation around the Z axis
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        transform.rotation = rotation;
        animator.SetBool("isCharging", true);
        state = State.Charging;

        //StartCoroutine(WaitForRotateAndMove(rotation));
    }

    IEnumerator WaitForRotateAndMove(Quaternion targetRotation)
    {
        
        // Smoothly rotate towards the target rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            Debug.Log("Angle: " + Quaternion.Angle(transform.rotation, targetRotation));
            Debug.Log("Target.rotation: " + targetRotation);
            Debug.Log("Current Transform.rotation: " + transform.rotation);
            Debug.Log("New Transform.rotation: " + Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.75f);
        // Call move function after rotation is finished
        animator.SetBool("isCharging", true);
        state = State.Charging;
    }

    IEnumerator Charge()
    {
        didCharge = true;

        float duration = 0.8f;
        Material material = spriteRenderer.material;
        string propertyName = "_ChargingTimer";
        float startValue = material.GetFloat(propertyName);

        // fade in
        while (chargeTimer < duration)
        {
            float newValue = Mathf.Lerp(startValue, 1, chargeTimer / duration);
            material.SetFloat(propertyName, newValue);
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        material.SetFloat(propertyName, duration);

        // Remember the end value of fade-in for fade-out
        float endValue = material.GetFloat(propertyName);

        // Reset timer for fade-out
        chargeTimer = 0;
        while (chargeTimer < duration)
        {
            float newValue = Mathf.Lerp(endValue, startValue, chargeTimer / duration);
            material.SetFloat(propertyName, newValue);
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        material.SetFloat(propertyName, 0);
        chargeTimer = 0;
    }

    void DidFinishChargeAnimation()
    {
        didCharge = false;
        animator.SetBool("isCharging", false);
        state = State.Attacking;
    }

    private void Reset()
    {
        targetPosition = Vector2.zero;
        StartCoroutine(ResetRotation());
    }

    IEnumerator ResetRotation()
    {

        Quaternion zeroRotation = Quaternion.AngleAxis(0, Vector3.forward);
        // Smoothly rotate towards the target rotation
        while (Quaternion.Angle(transform.rotation, zeroRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, zeroRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.75f);

        state = State.Idle;
        didAim = false;
    }


    //need something for OnTriggerStay incase the 'closest fish' is inside the enemy as the attack starts
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            if (state == State.Attacking)
            {
                if (alreadyHitTargets.Contains(collision.gameObject))
                {
                    return;
                }
                alreadyHitTargets.Add(collision.gameObject);
                fish.TakeDamage(enemySO.damage);
            }
        }
    }

    void PickRandomLocation()
    {

        float currentClosestFish = float.MaxValue;
        Fish currentClosestFishObject = null;
        bool lureFishInRange = false;
        List<Fish> fishInRangeList = new List<Fish>();
        List<LureClass> lureInRangeList = new List<LureClass>();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemySO.attackRange);
        if (hits != null)
        {
            foreach (Collider2D hit in hits)
            {
                if (hit.TryGetComponent<Fish>(out Fish fish))
                {
                    fishInRangeList.Add(fish);
                }
            }

            foreach (Fish f in fishInRangeList)
            {
                if (f.TryGetComponent<LureClass>(out LureClass lure))
                {
                    lureFishInRange = true;
                    lureInRangeList.Add(lure);
                }
            }

            if (lureFishInRange)
            {
                //iterate through lureList and find closest
                foreach (LureClass lure in lureInRangeList)
                {
                    float distanceOfFish = Mathf.Abs(Vector2.Distance(lure.transform.position, transform.position));
                    if (distanceOfFish <= currentClosestFish)
                    {
                        currentClosestFish = distanceOfFish;
                        if (lure.TryGetComponent<Fish>(out Fish f))
                        {
                            currentClosestFishObject = f;
                        }
                    }
                }
                AssignTargetedFish(currentClosestFish, currentClosestFishObject);

            }
            else
            {
                //no lure so find closest fish
                foreach (Fish f in fishInRangeList)
                {
                    float distanceOfFish = Mathf.Abs(Vector2.Distance(f.transform.position, transform.position));
                    if (distanceOfFish <= currentClosestFish)
                    {
                        currentClosestFish = distanceOfFish;
                        currentClosestFishObject = f;
                    }
                }
                AssignTargetedFish(currentClosestFish, currentClosestFishObject);
                    
            }
                
        }
        else
        {
            //No fish in range do something
            // Set a timer for like 3s, and then just default to 0,0 (or at least closer to fish)
        }

    }

    void AssignTargetedFish(float currentClosestFishDistance, Fish currentClosestFishObject)
    {
        if (currentClosestFishObject == null ) { return; }

        targetPosition = (Vector2)currentClosestFishObject.transform.position;

        float offsetMultiplier = Mathf.Clamp(currentClosestFishDistance / enemySO.attackRange, 0.25f, 0.5f);
        Vector2 direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();

        targetPosition = targetPosition + direction * offsetMultiplier;
       
        // Check if we go minimum distance, if not add distance
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget < minChargeDistance)
        {
            targetPosition = targetPosition + (direction * minChargeDistance);
        }

        // Clamp if we don't overshoot past the bounds. If we pass the bounds, we should just go straight to the fish
        if (
            targetPosition.x < tankBounds.min.x || targetPosition.x > tankBounds.max.x ||
            targetPosition.y < tankBounds.min.y || targetPosition.y > tankBounds.max.y
        )
        {
            // We got clamped, don't remove all offsets
            targetPosition = (Vector2)currentClosestFishObject.transform.position;
        }

        if (target == null && enemy.currentHP > 0)
        {
            target = Instantiate(targetSprite, targetPosition, Quaternion.identity);
        }
        else
        {
            target.transform.position = currentClosestFishObject.transform.position;
        }

        state = State.Aiming;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Change color as desired

        // Get the position of the game object
        Vector2 center = transform.position;

        // Draw the circle using Gizmos.DrawWireSphere
        Gizmos.DrawWireSphere(center, enemySO.attackRange);
    }

    void CheckDeath()
    {
        if (enemy.currentHP <= 0)
        {
            state = State.Dead;
        }
    }
}
