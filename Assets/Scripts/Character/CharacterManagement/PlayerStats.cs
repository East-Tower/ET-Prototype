using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager playerManager;
    public HealthBar healthBar;
    PlayerAttacker playerAttacker;

    AnimatorManager animatorManager;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerAttacker = GetComponent<PlayerAttacker>();
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
            playerManager.isDead = true;
        }
        else 
        {
            animatorManager.PlayTargetAnimation("GetHit_1", true, true);
            //临时添加, 受到伤害直接打断攻击状态
            playerManager.isAttacking = false;
            playerAttacker.chargingTimer = 0;
        }
    }

    public void StaminaRegen() 
    {
    
    }
}
