using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDiverBomb : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        if (canBeClicked)
        {
            amountEggClicked++;
            StartCoroutine(ClickCDTimer());
            if (amountEggClicked >= eggSprites.Count)
            {
                //play egg anim
                StartCoroutine(PlayerEggExplosionAnim());
            }
            else 
            {
                sr.sprite = eggSprites[amountEggClicked];
            }
        }
    }

    public int damage;
    public PoolObjectType poolType;
    [SerializeField] float cdToClickAgain;
    private SpriteRenderer sr;
    [SerializeField] List<Sprite> eggSprites;
    [SerializeField] List<Sprite> explosionEggSprites;
    private int amountEggClicked = 0;
    bool canBeClicked = true;

    Bounds tankBounds;
    public HashSet<GameObject> alreadyHitTargets = new HashSet<GameObject>();

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        tankBounds = GameObject.Find("Tank").GetComponent<PolygonCollider2D>().bounds;
    }

    public void OnEnable()
    {
        Spawn();
    }

    public void OnDisable()
    {
        sr.sprite = eggSprites[0];
        amountEggClicked = 0;
    }

    public void Spawn()
    {
        int randomX = Random.Range((int)-tankBounds.max.x, (int)tankBounds.max.x);
        transform.position = new Vector2(randomX, -tankBounds.max.y + 0.5f);
        sr.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Fish>(out Fish fish))
        {
            StartCoroutine(EggExplosionAnim());
            if (alreadyHitTargets.Contains(collision.gameObject))
            {
                return;
            }
            alreadyHitTargets.Add(collision.gameObject);
            fish.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("WaterLine"))
        {
            StartCoroutine(EggExplosionAnim());
        }
    }

    IEnumerator ClickCDTimer()
    {
        canBeClicked = false;
        yield return new WaitForSeconds(cdToClickAgain);
        canBeClicked = true;
    }

    IEnumerator PlayerEggExplosionAnim()
    {
        for (int i = 0; i < explosionEggSprites.Count; i++)
        { 
            sr.sprite = explosionEggSprites[i];
            yield return new WaitForSeconds(0.03f);
            if (i == explosionEggSprites.Count)
            {
                gameObject.SetActive(false);
            }
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
        yield return null;
    }

    IEnumerator EggExplosionAnim()
    {
        for (int i = amountEggClicked; i < eggSprites.Count; i++)
        { 
            sr.sprite = eggSprites[i];
            yield return new WaitForSeconds(0.02f);
        }

        for (int e = 0; e < explosionEggSprites.Count; e++)
        {
            sr.sprite = explosionEggSprites[e];
            yield return new WaitForSeconds(0.02f);
            if (e == explosionEggSprites.Count)
            {
                gameObject.SetActive(false);
            }
        }

        PoolManager.instance.DeactivateObjectInPool(gameObject);
        yield return null;
    }
}
