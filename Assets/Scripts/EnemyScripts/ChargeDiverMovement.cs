using JetBrains.Annotations;
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
    public float attackRange;
    private bool didAim;
    private bool didCharge;

    public GameObject targetSprite;

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
                break;
        }
    }

    void Move()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
            float t = 1f - Mathf.Clamp01(distanceToTarget / 3); // Clamping to ensure t is between 0 and 1
            float easedT = Mathf.SmoothStep(0f, 1f, t); // Apply easing function
            float easedMoveSpeed = Mathf.Lerp(enemySO.speed, 0f, easedT); // Interpolate movement speed based on eased t
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, easedMoveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            state = State.Resetting;
            StartCoroutine(ResetRotation());
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

        StartCoroutine(WaitForRotateAndMove(rotation));
    }

    IEnumerator WaitForRotateAndMove(Quaternion targetRotation)
    {
        // Smoothly rotate towards the target rotation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out  Fish fish))
        {
            if (state == State.Attacking)
            {
                fish.TakeDamage(enemySO.damage);
            }
        }
    }

    void PickRandomLocation()
    {
        //Vector2 randomPosition = Vector2.zero;
        //bool validPositionFound = false;

        //while (!validPositionFound)
        //{
        //    float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        //    float randomY = Random.Range(tankBounds.min.y + 1f, tankBounds.max.y - 1f);

        //    randomPosition = new Vector2(randomX, randomY);

        //    // Calculate the distance between the tank and the random position
        //    float distance = Vector2.Distance(randomPosition, transform.position);

        //    // Check if the distance is at least the minimum distance
        //    if (minChargeDistance <= distance && distance <= maxChargeDistance)
        //    {
        //        validPositionFound = true;
        //    }
        //}

        //targetPosition = randomPosition;
        //state = State.Aiming;

        Vector2 fishTargetPosition = Vector2.zero;
        float currentClosestFish = float.MaxValue;
        bool validFishFound = false;
        bool lureFishInTank = false;
        List<Fish> lureFishInTankList = new List<Fish>();

        while(validFishFound == false) 
        {
            foreach (Fish f in GameManager.instance.activeFish)
            {
                if (f.TryGetComponent<LureClass>(out LureClass lure))
                {
                    lureFishInTank = true;
                    lureFishInTankList.Add(f);
                }
                else 
                {
                    return;
                }
            }

            if (lureFishInTank)
            {
                foreach (Fish lureFish in lureFishInTankList)
                {
                    float distanceOfFish = Vector2.Distance(lureFish.transform.position, transform.position);
                    if (distanceOfFish < enemySO.attackRange)
                    {
                        if (distanceOfFish < currentClosestFish)
                        {
                            currentClosestFish = distanceOfFish;
                        }
                    }
                }
            }
            else
            {
                foreach (Fish f in GameManager.instance.activeFish)
                {

                }
            }
        }
        fishTargetPosition = currentClosestFish
    }

    void PickFishToAttack()
    {
        Vector2 randomPosition = Vector2.zero;
        bool validPositionFound = false;

        Fish[] fishInTank = GameObject.FindObjectsOfType<Fish>();
        while (!validPositionFound)
        { 
            
        }
    }
}
