using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab; // Reference to the bubble prefab
    public float minSpawnDelay = 1f; // Minimum time between spawns
    public float maxSpawnDelay = 10f; // Maximum time between spawns

    private float nextSpawnTime;

    void Start()
    {
        // Schedule the first spawn
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    void Update()
    {
        // Check if it's time to spawn a bubble
        if (Time.time >= nextSpawnTime)
        {
            SpawnBubble();
            // Schedule the next spawn
            nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    void SpawnBubble()
    {
        // Instantiate a bubble at a random position at the bottom of the screen
        Vector3 spawnPosition = new Vector3(Random.Range(-15f, 15f), -15f, 0f);
        Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
    }
}