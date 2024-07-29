using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] List<Enemy> enemyList;
    [SerializeField] float firstEnemySpawnTimerMin;
    [SerializeField] float firstEnemySpawnTimerMax;
    [SerializeField] float enemySpawnTimerMin;
    [SerializeField] float enemySpawnTimerMax;
    bool firstEnemySpawned = false;

    float firstEnemySpawnTimer;
    float enemySpawnTimer;

    GameObject tank;
    Bounds tankBounds;

    public delegate void EnemySpawnedEventHandler();
    public delegate void EnemyDefeatedEventHandler();

    public static event EnemySpawnedEventHandler OnEnemySpawned;
    public static event EnemyDefeatedEventHandler OnEnemyDefeated;

    private void Awake()
    {
        ChooseFirstEnemySpawnTimer();
        tank = GameObject.Find("Tank");
        tankBounds = tank.GetComponent<PolygonCollider2D>().bounds;
    }

    void ChooseFirstEnemySpawnTimer()
    {
        firstEnemySpawnTimer = Random.Range(firstEnemySpawnTimerMin, firstEnemySpawnTimerMax);
    }

    private void Update()
    {
        if (enemyList.Count >= 1)
        {
            if (firstEnemySpawned == false)
            {
                if (firstEnemySpawnTimer <= 0)
                {
                    SpawnEnemy();
                    firstEnemySpawned = true;
                }
                else
                {
                    firstEnemySpawnTimer -= Time.deltaTime;
                }
            }
            else
            {
                if (enemySpawnTimer <= 0)
                {
                    SpawnEnemy();
                }
                else
                {
                    enemySpawnTimer -= Time.deltaTime;
                }
            }
        }

    }

    void SpawnEnemy()
    {
        float randomX = Random.Range(tankBounds.min.x + 3f, tankBounds.max.x - 3f);
        float randomY = Random.Range(tankBounds.min.y + 1f, tankBounds.max.y - 1f);
        Vector2 randomSpawnPos = new Vector2(randomX, randomY);
        int randomEnemy = Random.Range(0, enemyList.Count);

        Enemy enemy = enemyList[randomEnemy];
        GameObject enemyObj = PoolManager.instance.GetPoolObject(enemy.gameObject);
        enemyObj.transform.localPosition = randomSpawnPos;

        enemySpawnTimer = Random.Range(enemySpawnTimerMin, enemySpawnTimerMax);
    }

    public static void EnemySpawned()
    {
        OnEnemySpawned?.Invoke();
    }

    public static void EnemyDefeated()
    {
        OnEnemyDefeated?.Invoke();
    }
}
