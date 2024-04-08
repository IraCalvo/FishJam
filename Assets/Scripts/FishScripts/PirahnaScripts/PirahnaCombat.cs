using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PirahnaCombat : MonoBehaviour
{
    Fish pirahna;
    FishSO pirahnaSO;
    FishState pirahnaState;
    Enemy targetEnemy;
    float attackCooldown;
    bool canAttack = true;
    bool isAttacking = false;
    bool attackRangeReached = false;
    bool pickedPos = false;
    bool oppositePointPicked = false;
    [SerializeField] Vector2 positionAroundEnemy;
    [SerializeField] Vector2 oppositePoint;
    SpriteRenderer sr;

    private void Awake()
    {
        pirahna = GetComponent<Fish>();
        pirahnaSO = pirahna.fishSO;
        pirahnaState = pirahna.fishState;
        attackCooldown = pirahnaSO.attackCooldown;
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (pirahnaState.GetCurrentState() == FishState.State.Combat)
        {
            ChooseEnemyToTarget();
            MoveToAttackRange();
        }
    }

    void ChooseEnemyToTarget()
    {
        Enemy[] enemiesInTank = FindObjectsOfType<Enemy>();
        if (enemiesInTank.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            Enemy closestEnemy = null;
            foreach (Enemy enemy in enemiesInTank)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }
            targetEnemy = closestEnemy;
        }
        else
        {
            return;
        }
    }

    void MoveToAttackRange()
    {
        if (targetEnemy != null)
        {
            //I need to a random position around the enemyPosition in a circle (the enemy as the center of the circle)
            if (pickedPos == false)
            {
                float randomAngle = Random.Range(0f, Mathf.PI * 2);
                float posX = targetEnemy.transform.position.x + Mathf.Cos(randomAngle) * pirahnaSO.attackRange;
                float posY = targetEnemy.transform.position.y + Mathf.Sin(randomAngle) * pirahnaSO.attackRange;
                positionAroundEnemy = new Vector2(posX, posY);
                pickedPos = true;
            }

            if (Vector2.Distance(transform.position, positionAroundEnemy) > 0.1f && attackRangeReached == false)
            {
                //move pirahna position
                float distanceToTarget = Vector2.Distance(transform.position, targetEnemy.transform.position);
                //float t = 1f - Mathf.Clamp01(distanceToTarget / 10f);
                //float easedT = Mathf.SmoothStep(0f, 1f, t);
                //float easedCombatSpeed = Mathf.Lerp(pirahnaSO.combatMoveSpeed, 0f, easedT);
                transform.position = Vector2.MoveTowards(transform.position, positionAroundEnemy, pirahnaSO.combatMoveSpeed * Time.fixedDeltaTime);
                Debug.Log("still going to range");
                SpriteDirection(positionAroundEnemy);
            }
            else 
            {
                attackRangeReached = true;
                Attack();
            }
        }
        else
        {
            return;
        }
    }

    void Attack()
    {
        if (oppositePointPicked == false)
        {
            float xDistance = targetEnemy.transform.position.x - transform.position.x;
            float yDistance = targetEnemy.transform.position.y - transform.position.y;
            oppositePoint = new Vector2(targetEnemy.transform.position.x + xDistance, targetEnemy.transform.position.y + yDistance);
            Debug.Log("This is the fish current pos:" + transform.position + "This is the opposite point to dash to:" + oppositePoint);
            oppositePointPicked = true;
        }

        if (Vector2.Distance(transform.position,oppositePoint) >= 0.1f && canAttack)
        {
            transform.position = Vector2.MoveTowards(transform.position, oppositePoint, pirahnaSO.combatMoveSpeed * Time.fixedDeltaTime);
            isAttacking = true;
            Debug.Log("is dashing");
            SpriteDirection(oppositePoint);
        }
        else
        {
            Debug.Log("Resetting");
            pickedPos = false;
            attackRangeReached = false;
            isAttacking = false;
            oppositePointPicked = false;
            CooldownTimerCoroutine();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isAttacking)
        {
            targetEnemy.TakeDamage(pirahnaSO.damage);
        }
    }

    IEnumerator CooldownTimerCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator Rotate(Vector2 targetPosition)
    {
        // Calculate the direction vector from the object to the target
        Vector2 direction = targetPosition - (Vector2)transform.position;

        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a Quaternion rotation around the Z axis
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        // Smoothly rotate towards the target rotation
        while (Quaternion.Angle(transform.rotation, rotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
            yield return null;
        }
    }

    void SpriteDirection(Vector2 targetPosition)
    {
        if (transform.position.x < targetPosition.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }
}
