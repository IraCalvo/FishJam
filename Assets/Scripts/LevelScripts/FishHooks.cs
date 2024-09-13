using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FishHook : MonoBehaviour
{
    [Header("Base Hook Info and Stats")]
    [SerializeField] float hookSpeed;
    [SerializeField] float minHookTimer;
    [SerializeField] float maxHookTimer;
    float hookTimer;

    Collider2D hookCollider;
    bool isExplosiveHook;
    bool hookGoingUp;
    bool fishIsHooked;
    Fish hookedFish;
    Transform parentTransform;
    FoodType foodType = FoodType.FishHook;

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
    }

    private void OnDisable()
    {
        fishIsHooked = false;
        hookedFish = null;
        hookGoingUp = false;
    }

    private void Update()
    {
        RotateHook();
        if (hookTimer > 0)
        {
            hookTimer -= Time.deltaTime;
        }
        else
        {
            hookGoingUp = true;
        }

        if (hookGoingUp)
        {
            parentTransform.Translate(hookSpeed * Time.deltaTime * Vector2.up);
            hookedFish.transform.Translate(hookSpeed * Time.deltaTime * Vector2.up);
        }
    }

    void RotateHook()
    {
        float time = Mathf.PingPong(Time.time * rotationSpeed, 1f);
        float targetAngle = Mathf.LerpAngle(angleA, angleB, time);
        transform.rotation = Quaternion.Euler(0,0, targetAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WaterLine"))
        {
            SFXManager.instance.PlaySFX(SoundType.Splash);
        }

        if (collision.gameObject.CompareTag("WaterLine") && hookGoingUp)
        {
            if (fishIsHooked)
            {
                PoolManager.instance.DeactivateObjectInPool(hookedFish.gameObject);
                PoolManager.instance.DeactivateObjectInPool(parentTransform.gameObject);
                SFXManager.instance.PlaySFX(SoundType.Splash);
            }
        }

        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            if (fish.fishState.GetCurrentState() == FishState.State.Hungry && !fishIsHooked)
            {
                HookedFish();
                hookedFish = fish;
                fish.fishState.SetStateTo(FishState.State.Hooked);
            }
        }
    }

    void HookedFish()
    {
        fishIsHooked = true;
        if (isExplosiveHook)
        {
            //Cause explosion here
        }
        else
        {
            hookGoingUp = true;
        }
    }
}
