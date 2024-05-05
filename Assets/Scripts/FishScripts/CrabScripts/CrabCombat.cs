using System.Collections;
using System.Collections.Generic;
//using TreeEditor;
using UnityEngine;

public class CrabCombat : MonoBehaviour
{
    Fish crab;
    FishSO crabSO;
    FishState crabState;
    Enemy targetEnemy;
    Animator animator;
    bool canAttack = true;


    private void Awake()
    {
        crab = GetComponent<Fish>();
        crabSO = crab.fishSO;
        crabState = crab.fishState;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    { 
        if(crabState.GetCurrentState() == FishState.State.Combat)
        {
            ChooseEnemyToTarget();
            MoveToAttackRange();
        }
    }

    void ChooseEnemyToTarget()
    {
        Debug.Log("Enemy Detected");
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
            Debug.Log("Trying to move to enemy");
            Vector2 targetPos = new(targetEnemy.transform.position.x, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, targetPos, crabSO.combatMoveSpeed * Time.fixedDeltaTime);
        }
        else 
        {
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canAttack && crabState.GetCurrentState() == FishState.State.Combat)
        {
            Debug.Log(targetEnemy);
            targetEnemy.TakeDamage(crabSO.damage);
            animator.SetTrigger("CrabAtk");
            StartCoroutine(CooldownTimerCoroutine());
        }
    }

    public void AttackAnimDone()
    {
        animator.SetTrigger("CrabIdle");
    }

    IEnumerator CooldownTimerCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(crabSO.attackCooldown);
        canAttack = true;
    }
}
