using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;
    private void Awake()
    {
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
        }
        else
        {
            animator.Play("GetHit_1");
        }
    }
}
