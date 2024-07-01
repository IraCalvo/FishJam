using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDiverMovement : MonoBehaviour
{

    public float movementSpeed;
    public float nextAttackTimer;
    public List<BombDiverBomb> bombDiverBombList;

    private Rigidbody2D rb;
    private Collider2D collider;

    private bool walkingRight;
    private float nextAttackTimerInternal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        walkingRight = true;
        nextAttackTimerInternal = nextAttackTimer;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        rb.velocity = new Vector2(walkingRight ? movementSpeed : -movementSpeed, rb.velocity.y);

        if (walkingRight && transform.position.x >= 15.0)
        {
            walkingRight = false;
        }
        else if (!walkingRight && transform.position.x <= -15.0)
        {
            walkingRight = true;
        }
    }

    void Attack()
    {
        if (nextAttackTimerInternal <= 0.0f)
        {
            SpawnBomb();
            nextAttackTimerInternal = nextAttackTimer;
        }
        else
        {
            nextAttackTimerInternal -= Time.deltaTime;
        }
    }

    void SpawnBomb()
    {
        int randomBombIndex = Random.Range(0, bombDiverBombList.Count);
        BombDiverBomb randomBomb = bombDiverBombList[randomBombIndex];
        randomBomb.Spawn();
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
