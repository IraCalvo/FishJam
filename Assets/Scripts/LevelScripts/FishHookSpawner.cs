using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHookSpawner : MonoBehaviour
{
    Bounds tankBounds;
    Camera mainCam;
    [SerializeField] private float minHookSpawnTime;
    [SerializeField] private float maxHookSpawnTime;
    float spawnTimer;
    bool isSpawningHook = false;

    Vector2 targetPosition;
    Vector2 randX;

    [SerializeField] GameObject hookPing;
    GameObject recentHookPing;

    private void Awake()
    {
        mainCam = Camera.main;
        tankBounds = GameObject.Find("Tank").GetComponent<PolygonCollider2D>().bounds;
        ChooseSpawnTimer();
    }

    private void Update()
    {
        if (spawnTimer <= 0 && !isSpawningHook)
        {
            //spawn hook
            isSpawningHook = true;
            StartCoroutine(SpawnHookPing());
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    void ChooseSpawnTimer()
    {
        spawnTimer = Random.Range(minHookSpawnTime, maxHookSpawnTime);
        Debug.Log("Spawn Timer is:" + spawnTimer);
    }

    IEnumerator SpawnHookPing()
    {
        randX = RandomX();
        targetPosition = DropHook(randX);

        recentHookPing = Instantiate(hookPing, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(3.5f);
        GameObject hookObj = PoolManager.instance.GetPoolObject(PoolObjectType.FishHook);
        SpawnHook(hookObj);
    }

    void SpawnHook(GameObject hookToSpawn)
    {
        //setting spawn pos of hook
        Destroy(recentHookPing);
        hookToSpawn.transform.position = randX;
        hookToSpawn.GetComponentInChildren<FishHook>().spawnYPos = targetPosition;

        ChooseSpawnTimer();
        isSpawningHook = false;
    }

    private Vector2 RandomX()
    {
        //Bounds viewportBounds = GetViewportBounds(mainCam);
        Vector2 spawnPos = new Vector2(Random.Range(tankBounds.min.x + 5, tankBounds.max.x - 5), 20f);
        return spawnPos;
    }

    //+3 so that the hook doesnt go into the sand
    //TODO: tank collider with offset on Y to account for sky/ground
    private Vector2 DropHook(Vector2 spawnPos)
    {
        //Bounds viewportBounds = GetViewportBounds(mainCam);
        float depth = Random.Range(tankBounds.min.y - 5, tankBounds.max.y + 3);
        depth = Mathf.Clamp(depth, tankBounds.min.y, tankBounds.max.y + 3);
        Vector2 targetPos = new Vector2(spawnPos.x, depth);
        return targetPos;
    }

    //Bounds GetViewportBounds(Camera cam)
    //{
    //    Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
    //    Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

    //    Bounds bounds = new Bounds();
    //    bounds.SetMinMax(bottomLeft, topRight);
    //    return bounds;
    //}
}
