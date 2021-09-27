using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    Rigidbody rig;
    Transform cameraObject;

    public int comboCount;
    public float attackTimer;
    public float internalDuration = 2f;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        rig = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            attackTimer = 0;
            comboCount = 0;
        }
    }

    public void HandleRegularAttack(WeaponItem weapon)
    {
        if (!playerManager.isAttacking && playerManager.isGround) 
        {
            playerManager.isAttacking = true;
            comboCount++;
            if (comboCount > 4) 
            {
                comboCount = 1;
            }
            attackTimer = internalDuration;

            if (comboCount == 1)
            {
                animatorManager.PlayTargetAnimation(weapon.SW_Regular_Attack_4, true, true);
            }
            else if (comboCount == 2)
            {
                animatorManager.PlayTargetAnimation(weapon.SW_Regular_Attack_1, true, true);
            }
            else if (comboCount == 3)
            {
                animatorManager.PlayTargetAnimation(weapon.SW_Regular_Attack_2, true, true);
            }
            else if (comboCount == 4)
            {
                animatorManager.PlayTargetAnimation(weapon.SW_Regular_Attack_3, true, true);
            }
        }

        
    }

    public void HandleSpecialAttack(WeaponItem weapon)
    {
        if (!playerManager.isAttacking && playerManager.isGround)
        {
            playerManager.isAttacking = true;
            attackTimer = internalDuration;

            if (comboCount == 0)
            {
                animatorManager.PlayTargetAnimation(weapon.SW_Regular_Attack_4, true, true);
                comboCount++;
            }
            else 
            {
                comboCount++;

                if (comboCount == 2)
                {
                    // Charging Attack
                    if (inputManager.spAttack_Input)
                    {
                        animatorManager.PlayTargetAnimation(weapon.SW_Special_Attack_3, true, true);
                    }
                }
                else if (comboCount == 3)
                {
                    animatorManager.PlayTargetAnimation(weapon.SW_Special_Attack_4, true, true);
                }
                else if (comboCount == 4)
                {
                    animatorManager.PlayTargetAnimation(weapon.SW_Special_Attack_2, true, true);
                    playerManager.isAttacking = true;
                }
                else if (comboCount == 5)
                {
                    animatorManager.PlayTargetAnimation(weapon.SW_Special_Attack_1, true, true);
                }
                comboCount = 0;
            }
        }

        //rig.velocity = new Vector3(0, rig.velocity.y, 0);
    }
}
