using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyManager enemyManager;
    Animator animator;
    //IdleState idleState;
    EnemyAnimatorManager animatorManager;

    //Boss
    public int stage; // 0 - 1, 1-2...
    public int phase2RequiredHealth;

    public int staminaGauge;
    public int curStamina;

    bool phaseChanged;

    public GameObject phaseVFX;
    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<EnemyAnimatorManager>();
        //idleState = GetComponentInChildren<IdleState>();
    }
    private void Start()
    {
        currHealth = maxHealth;
    }

    private void Update()
    {
        if (currHealth > phase2RequiredHealth)
        {
            stage = 0;
        }
        else if (currHealth <= phase2RequiredHealth && !phaseChanged)
        {
            //播放转阶段动画
            animatorManager.PlayTargetAnimation("FallDown", true);
            //无法被伤害
            stage = 1;
            phaseChanged = true;
            phaseVFX.SetActive(true);
            staminaGauge = staminaGauge / 2; //二阶段的耐力上限改变
        }

        if (curStamina >= staminaGauge)  //耐力低于设定值时会播放个倒地动画
        {
            animator.Play("GetHit_1"); //受身动画
            curStamina = 0;
        }
    }
    public void TakeDamage(int damage, CharacterStats characterStats = null)
    {
        if (!phaseChanged) 
        {
            currHealth = currHealth - damage;
            enemyManager.curTarget = characterStats;
            curStamina += damage;
            //idleState.HandleRotateTowardsTarger(enemyManager);
            if (currHealth <= 0)
            {
                currHealth = 0;
                animator.Play("Dead");
                enemyManager.isDead = true;
            }
            else
            {
                //if (!enemyManager.isImmuneAttacking)
                //{
                //    animator.Play("GetHit_1");
                //}
            }
        }
    }
}
