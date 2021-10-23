using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    Animator animator;
    IdleState idleState;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        idleState = GetComponentInChildren<IdleState>();
    }
    private void Start()
    {
        currHealth = maxHealth;
    }
    public void TakeDamage(int damage, CharacterStats characterStats = null)
    {
        currHealth = currHealth - damage;
        enemyManager.curTarget = characterStats;
        idleState.HandleRotateTowardsTarger(enemyManager);
        if (currHealth <= 0)
        {
            currHealth = 0;
            animator.Play("Dead");
            enemyManager.isDead = true;
        }
        else
        {
            if (!enemyManager.isImmuneAttacking) 
            {
                animator.Play("GetHit_1");
            }
        }
    }
}
