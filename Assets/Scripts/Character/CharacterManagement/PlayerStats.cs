using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public HealthBar healthBar;

    AnimatorManager animatorManager;

    private void Awake()
    {
        animatorManager = GetComponentInChildren<AnimatorManager>();
    }
    private void Start()
    {
        currHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    public void TakeDamage(int damage) 
    {
        currHealth = currHealth - damage;
        healthBar.SetCurrentHealth(currHealth);

        if (currHealth <= 0)
        {
            currHealth = 0;
            animatorManager.PlayTargetAnimation("Dead", true, true);
        }
        else 
        {
            animatorManager.PlayTargetAnimation("GetHit_1", true, true);
        }
    }
}
