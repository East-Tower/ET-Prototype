using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    Sample_VFX sample_VFX;


    //普通攻击
    public int comboCount;
    public float attackTimer;
    public float internalDuration = 2f;
    //蓄力攻击
    public float chargingTimer;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        sample_VFX = GetComponentInChildren<Sample_VFX>();
    }
    private void Update()
    {
        AttackComboTimer();
        ChargingTimer();
    }
    public void HandleRegularAttack(WeaponItem weapon) //左键普攻
    {
        if (!playerManager.isAttacking && playerManager.isGround) 
        {
            playerManager.isAttacking = true;
            attackTimer = internalDuration;
            comboCount++;
            if (comboCount > 4)
            {
                comboCount = 1;
            }
            //播放指定的攻击动画
            animatorManager.PlayTargetAnimation(weapon.regularSkills[comboCount-1].skillName, true, true);            

        }
    }
    public void HandleSpecialAttack(WeaponItem weapon) //右键
    {
        if (!playerManager.isAttacking && playerManager.isGround)
        {
            playerManager.isAttacking = true;
            attackTimer = internalDuration;

            if (comboCount == 0)
            {
                animatorManager.PlayTargetAnimation(weapon.regularSkills[comboCount].skillName, true, true);
                comboCount++;
            }
            else 
            {
                animatorManager.PlayTargetAnimation(weapon.specialSkills[comboCount-1].skillName, true, true);
                sample_VFX.curVFX_List[comboCount-1].Play();
                comboCount = 0;
            }
        }
        //rig.velocity = new Vector3(0, rig.velocity.y, 0);
    }
    public void AttackComboTimer() 
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 0;
            comboCount = 0;
        }
    }
    public void ChargingTimer() 
    {
        if (playerManager.isCharging) 
        {
            chargingTimer += Time.deltaTime;
            if (sample_VFX.curVFX_List[4].isStopped)
            {
                sample_VFX.curVFX_List[4].Play();
            }
            if (!inputManager.spAttack_Input) 
            {
                float curTime = chargingTimer;
                chargingTimer = 0;
                if (curTime > 0.9f)
                {
                    animatorManager.animator.SetBool("isCharging", false);
                    animatorManager.animator.SetBool("isUsingRootMotion", false);
                    sample_VFX.curVFX_List[5].Play();
                    playerManager.isAttackDashing = true;
                }
                else 
                {
                    sample_VFX.curVFX_List[0].Stop();
                    playerManager.isAttacking = false;
                }
            }
        }
    }
}
