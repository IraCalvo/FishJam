using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.InputSystem;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }
    [SerializeField] GameObject fishPrefab;
    [SerializeField] int fishCost;
    public Bounds tankBounds;
    Camera mainCamera;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SpawnFish(int numberPressed)
    {
        if (BankManager.Instance.currentMoneyAmount >= fishCost)
        {
            BankManager.Instance.RemoveMoney(fishCost);
            GameObject fishGameObject = Instantiate(fishPrefab);
            // TODO:
            // This should eventually select the specific fish prefab.
            int index = numberPressed - 1;
            //
            DropFish(fishGameObject);
        }
    }

    private void DropFish(GameObject fishGameObject)
    {
        FishMovement fishScript = fishGameObject.GetComponent<FishMovement>();
        Bounds viewportBounds = GetViewportBounds(mainCamera);
        Debug.Log(viewportBounds);
        Vector2 spawnPosition = new Vector2(Random.Range(viewportBounds.min.x, viewportBounds.max.x), 20f);

        fishGameObject.transform.position = spawnPosition;

        float depth = Random.Range(viewportBounds.min.y, viewportBounds.max.y);
        Vector2 targetPosition = new Vector2(spawnPosition.x, depth);
        fishScript.targetPosition = targetPosition;
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
