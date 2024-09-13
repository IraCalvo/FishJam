using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookSpawner : MonoBehaviour
{
    Bounds tankBounds;
    Camera mainCam;
    [SerializeField] private float minHookSpawnTime;
    [SerializeField] private float maxHookSpawnTime;
    private float spawnTimer;
    Transform parentHookObj;

    private void Awake()
    {
        tankBounds = GameObject.Find("Tank").GetComponent<PolygonCollider2D>().bounds;
        ChooseSpawnTimer();
    }

    private void Update()
    {
        if (spawnTimer <= 0)
        {
            ChooseSpawnTimer();

            //spawn hook
            GameObject hookObj = PoolManager.instance.GetPoolObject(PoolObjectType.FishHook);
            SpawnHook(hookObj);
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    void ChooseSpawnTimer()
    {
        spawnTimer = Random.Range(minHookSpawnTime, maxHookSpawnTime);
    }

    void SpawnHook(GameObject hookToSpawn)
    {
        //setting spawn pos of hook
        Vector2 randX = RandomX();
        Vector2 targetPosition = DropHook(randX);
        parentHookObj = hookToSpawn.transform.parent;
        parentHookObj.transform.position = targetPosition;
    }

    private Vector2 RandomX()
    {
        Bounds viewportBounds = GetViewportBounds(mainCam);
        Vector2 spawnPos = new Vector2(Random.Range(viewportBounds.min.x, viewportBounds.max.x), 20f);
        return spawnPos;
    }

    private Vector2 DropHook(Vector2 spawnPos)
    {
        Bounds viewportBounds = GetViewportBounds(mainCam);
        float depth = Random.Range(viewportBounds.min.y, tankBounds.max.y);
        depth = Mathf.Clamp(depth, tankBounds.min.y, viewportBounds.max.y);
        Vector2 targetPos = new Vector2(spawnPos.x, depth);
        return targetPos;
    }

    Bounds GetViewportBounds(Camera cam)
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        Bounds bounds = new Bounds();
        bounds.SetMinMax(bottomLeft, topRight);
        return bounds;
    }
}
