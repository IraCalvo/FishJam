using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }
    [SerializeField] GameObject fishPrefab;
    public Bounds tankBounds;
    Camera mainCamera;

    private void Awake()
    {
        Instance = this;
        tankBounds = GameObject.Find("Tank").GetComponent<PolygonCollider2D>().bounds;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SpawnFish(GameObject fishObject)
    {
        if (fishObject.TryGetComponent<Fish>(out Fish fish))
        {
            if (BankManager.Instance.CanAfford(fish.fishSO.price))
            {
                SFXManager.instance.PlaySFX(SoundType.ButtonPressed);
                BankManager.Instance.RemoveMoney(fish.fishSO.price);
                GameObject obj = PoolManager.instance.GetPoolObject(fish.gameObject);

                SetSpawnPosition(obj);
            }
            else
            {
                SFXManager.instance.PlaySFX(SoundType.NotEnoughMoney);
            }
        }
    }

    public void SetSpawnPosition(GameObject fishGameObject)
    {
        Vector2 randomX = RandomX();
        Vector2 targetPosition = DropFish(randomX);
        fishGameObject.transform.position = randomX;

        if (fishGameObject.TryGetComponent<FishMovement>(out FishMovement fishMovement))
        {
            fishMovement.targetPosition = targetPosition;
        }
        else if (fishGameObject.TryGetComponent<CrabMovement>(out CrabMovement crabMovement) 
            || fishGameObject.TryGetComponent<SnailMovement>(out SnailMovement snailMovement))
        {
            // Do nothing. The Crab will fall and and change to normal state
        }
        
    }

    private Vector2 RandomX()
    {
        Bounds viewportBounds = GetViewportBounds(mainCamera);
        Vector2 spawnPosition = new Vector2(Random.Range(viewportBounds.min.x, viewportBounds.max.x), 20f);
        return spawnPosition;
    }

    private Vector2 DropFish(Vector2 spawnPosition)
    {
        Bounds viewportBounds = GetViewportBounds(mainCamera);
        float depth = Random.Range(viewportBounds.min.y, viewportBounds.max.y);
        depth = Mathf.Clamp(depth, tankBounds.min.y, tankBounds.max.y);
        Vector2 targetPosition = new Vector2(spawnPosition.x, depth);
        return targetPosition;
    }

    Bounds GetViewportBounds(Camera camera)
    {
        Vector3 bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));

        Bounds bounds = new Bounds();
        bounds.SetMinMax(bottomLeft, topRight);
        return bounds;
    }
}
