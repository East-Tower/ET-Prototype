using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    public Sample_VFX sample_VFX_R;
    public Sample_VFX sample_VFX_S;


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
    }
    private void Update()
    {
        AttackComboTimer();
        ChargingTimer();
    }
    public void HandleRegularAttack(WeaponItem weapon) //左键普攻
    {
        //使用指定武器信息中的普通攻击
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
            sample_VFX_R.curVFX_List[comboCount - 1].Play();
        }
    }
    public void HandleSpecialAttack(WeaponItem weapon) //右键特殊攻击
    {
        if (!playerManager.isAttacking && playerManager.isGround)
        {
            playerManager.isAttacking = true;
            attackTimer = internalDuration;

            if (comboCount == 0)
            {
                //右键的第一下就是普通的第一下
                animatorManager.PlayTargetAnimation(weapon.regularSkills[comboCount].skillName, true, true);
                comboCount++;
            }
            else 
            {
                //其余都播放特殊攻击的动作
                animatorManager.PlayTargetAnimation(weapon.specialSkills[comboCount-1].skillName, true, true);
                sample_VFX_S.curVFX_List[comboCount-1].Play();
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
    public void ChargingTimer() //蓄力计时器(当前只针对特殊攻击1的情况进行了使用)
    {
        if (playerManager.isCharging) 
        {
            chargingTimer += Time.deltaTime;
            if (sample_VFX_S.curVFX_List[4].isStopped)
            {
                sample_VFX_S.curVFX_List[4].Play();
            }
            if (!inputManager.spAttack_Input) 
            {
                float curTime = chargingTimer;
                chargingTimer = 0;
                if (curTime > 0.9f)
                {
                    animatorManager.animator.SetBool("isCharging", false);
                    animatorManager.animator.SetBool("isUsingRootMotion", false);
                    sample_VFX_S.curVFX_List[5].Play();
                    playerManager.isAttackDashing = true;
                }
                else 
                {
                    sample_VFX_S.curVFX_List[0].Stop();
                    playerManager.isAttacking = false;
                }
            }
        }
    }
}
