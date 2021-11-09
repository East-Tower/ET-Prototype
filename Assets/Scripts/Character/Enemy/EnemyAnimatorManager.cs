using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MainAnimatorManager
{
    EnemyManager enemyManager;
    Boss_CombatStanceState boss_CombatStanceState;
    public Vector3 velocity;
    public Vector3 deltaPosition;

    public Collider damageCollider;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        boss_CombatStanceState = GetComponentInChildren<Boss_CombatStanceState>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyManager.enemyRig.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        velocity = deltaPosition / delta;
        enemyManager.enemyRig.velocity = velocity * enemyManager.moveSpeed ;

        if (enemyManager.isRotatingWithRootMotion) 
        {
            enemyManager.transform.rotation *= animator.deltaRotation;
        }
    }

    private void CheckingPlayerPosition() 
    {
        if (enemyManager.curTargetAngle == 0 && enemyManager.curTargetDistance == 0) //Combo Check
        {
            animator.SetBool("canCombo", true);
        }
    }

    private void shoutStun() 
    {
        Collider[] targetInArea = Physics.OverlapSphere(transform.position, enemyManager.shoutRadius, enemyManager.playerLayer);
        foreach (Collider player in targetInArea)
        {
            player.GetComponent<PlayerManager>().GetDebuff(3f);
        }
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }


    private void RangeAttack() 
    {
        enemyManager.HandleRangeAttack();
    }
}
