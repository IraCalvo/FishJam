using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDiverMovement : MonoBehaviour
{
    Enemy chickenEnemy;
    [SerializeField] BombDiverBomb bombPrefab;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private bool walkingRight;
    private float nextAttackTimerInternal;

    private void Awake()
    {
        chickenEnemy = GetComponent<Enemy>();   
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        walkingRight = true;
        nextAttackTimerInternal = chickenEnemy.enemySO.attackCD;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        rb.velocity = new Vector2(walkingRight ? chickenEnemy.enemySO.speed : -chickenEnemy.enemySO.speed, rb.velocity.y);

        if (walkingRight && transform.position.x >= 15.0)
        {
            walkingRight = false;
            sr.flipX = true;
        }
        else if (!walkingRight && transform.position.x <= -15.0)
        {
            walkingRight = true;
            sr.flipX = false;
        }
    }

    void Attack()
    {
        if (nextAttackTimerInternal <= 0.0f)
        {
            SpawnBomb();
            nextAttackTimerInternal = chickenEnemy.enemySO.attackCD;
        }
        else
        {
            nextAttackTimerInternal -= Time.deltaTime;
        }
    }

    void SpawnBomb()
    {
        PoolManager.instance.GetPoolObject(bombPrefab.poolType);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
        }
    }
}
