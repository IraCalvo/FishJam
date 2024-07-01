using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDiverBomb : MonoBehaviour
{
    public int damage;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        BombDiverBomb bombCopy = Instantiate(this);
        int randomX = Random.Range(-12, 12);
        bombCopy.transform.position = new Vector2(randomX, -20);
        bombCopy.sr.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            fish.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
