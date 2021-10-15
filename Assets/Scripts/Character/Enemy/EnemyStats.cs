using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    Animator animator;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        currHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currHealth = currHealth - damage;

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
