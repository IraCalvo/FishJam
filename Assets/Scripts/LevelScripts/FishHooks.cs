using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public enum HookState
{ 
    None,
    Spawning,
    Rotating,
    MovingUp
}

public class FishHook : MonoBehaviour
{
    [Header("Base Hook Info and Stats")]
    [SerializeField] float hookSpeed;
    [SerializeField] float minHookTimer;
    [SerializeField] float maxHookTimer;
    float hookTimer;
    public Vector2 spawnYPos;
    public float spawnSpeed;
    public bool canHookFish = false;
    public HookState hookState;

    Collider2D hookCollider;
    bool isExplosiveHook;
    bool fishIsHooked;
    Fish hookedFish;
    Transform parentTransform;
    public FoodType foodType = FoodType.FishHook;

    [Header("HookAnim Stuff")]
    [SerializeField] float minRotationSpeed;
    [SerializeField] float maxRotationSpeed;
    float rotationSpeed;
    [SerializeField] float angleA;
    [SerializeField] float angleB;


    private void Awake()
    {
        parentTransform = transform.parent;
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        hookCollider = GetComponent<Collider2D>();
        hookTimer = Random.Range(minHookTimer, maxHookTimer);
        hookState = HookState.Spawning;
    }

    private void OnDisable()
    {
        fishIsHooked = false;
        hookedFish = null;
    }

    private void Update()
    {
        switch (hookState)
        {
            case HookState.Spawning:
                MoveHookToSpawnPos();
                break;
            case HookState.Rotating:
                RotateHook();
                break;
            case HookState.MovingUp:
                MoveHookUpwards();
                break;
        }
    }

    void RotateHook()
    {
        float time = Mathf.PingPong(Time.time * rotationSpeed, 1f);
        float targetAngle = Mathf.LerpAngle(angleA, angleB, time);
        transform.rotation = Quaternion.Euler(0,0, targetAngle);

        if (hookTimer > 0)
        {
            hookTimer -= Time.deltaTime;
        }
        else
        {
            hookState = HookState.MovingUp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WaterLine"))
        {
            SFXManager.instance.PlaySFX(SoundType.Splash);
        }

        if (collision.gameObject.CompareTag("WaterLine") && hookState == HookState.MovingUp)
        {
            if (fishIsHooked)
            {
                PoolManager.instance.DeactivateObjectInPool(hookedFish.gameObject);
                PoolManager.instance.DeactivateObjectInPool(parentTransform.gameObject);
                SFXManager.instance.PlaySFX(SoundType.Splash);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            if (fish.fishState.GetCurrentState() == FishState.State.Hungry && !fishIsHooked && canHookFish)
            {
                HookedFish();
                hookedFish = fish;
                fish.fishState.SetStateTo(FishState.State.Hooked);
            }
        }
    }

    void HookedFish()
    {
        canHookFish = false;
        fishIsHooked = true;
        if (isExplosiveHook)
        {
            //Cause explosion here
        }
        else
        {
            hookState = HookState.MovingUp;
        }
    }

    void MoveHookToSpawnPos()
    {
        if (Vector2.Distance(parentTransform.transform.position, spawnYPos) >= 0.1f && hookState != HookState.MovingUp)
        {
            float distanceToTarget = Vector2.Distance(parentTransform.transform.position, spawnYPos);
            float t = 1f - Mathf.Clamp01(distanceToTarget / 5);
            float easedT = Mathf.SmoothStep(0f, 1f, t);
            float easedMoveSpeed = Mathf.Lerp(hookSpeed, 0.5f, easedT);
            parentTransform.transform.position = Vector2.MoveTowards(parentTransform.transform.position, spawnYPos, easedMoveSpeed * Time.deltaTime);
        }
        else
        {
            canHookFish = true;
            hookState = HookState.Rotating;
        }
    }

    void MoveHookUpwards()
    {
        parentTransform.Translate(hookSpeed * Time.deltaTime * Vector2.up);
        if (hookedFish != null)
        {
            hookedFish.transform.Translate(hookSpeed * Time.deltaTime * Vector2.up);
        }
    }
}
