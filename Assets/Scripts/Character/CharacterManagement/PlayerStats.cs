using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    PlayerManager playerManager;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    PlayerAttacker playerAttacker;

    AnimatorManager animatorManager;

    public float currStamina;
    [SerializeField] float maxStamina = 100;
    [SerializeField] float staminaRegen = 5;

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

        currStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
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
    public void CostStamina(float cost) 
    {
        currStamina = currStamina - cost;
        staminaBar.SetCurrentStamina(currStamina);
    }

    public void StaminaRegen() 
    {
        if (!playerManager.isInteracting && !playerManager.isSprinting &&currStamina < maxStamina) 
        {
            currStamina = currStamina + staminaRegen * Time.deltaTime;
        }
        staminaBar.SetCurrentStamina(currStamina);
    }
}
