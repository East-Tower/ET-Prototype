using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public CombatStanceState combatStanceState;

    public EnemyAttackAction[] enemyAttacks;
    public EnemyAttackAction curAttack;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.curTarget.transform.position - transform.position;
        float distanceFromTarget = Vector3.Distance(enemyManager.curTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        HandleRotateTowardsTarger(enemyManager);

        if (enemyManager.isPreformingAction)
            return combatStanceState;

        if (curAttack != null)
        {
            //If we are too close to the enemy to preform attack, get a new attack
            if (distanceFromTarget < curAttack.minDistanceNeedToAttack)
            {
                return this;
            }
            else if (distanceFromTarget < curAttack.maxDistanceNeedToAttack)
            {
                if (viewableAngle <= curAttack.maxAttackAngle && viewableAngle >= curAttack.minAttackAngle)
                {
                    if (enemyManager.curRecoveryTime <= 0 && enemyManager.isPreformingAction == false)
                    {
                        //确认是否为霸体状态的攻击
                        if (curAttack.isImmune)
                            enemyManager.isImmuneAttacking = true;
                        else
                            enemyManager.isImmuneAttacking = false;

                        enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
                        enemyAnimatorManager.PlayTargetAnimation(curAttack.actionAnimation, true);
                        enemyManager.isPreformingAction = true;
                        enemyManager.curRecoveryTime = curAttack.recoveryTime;
                        curAttack = null;
                        return combatStanceState;
                    }
                }
            }
        }
        else 
        {
            GetNewAttack(enemyManager);
        }

        return combatStanceState;
    }

    private void GetNewAttack(EnemyManager enemyManager) //攻击从设置好的攻击列表中随机挑选下一次的攻击动画
    {
        Vector3 targetDirection = enemyManager.curTarget.transform.position - transform.position;
        float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
        float distanceFromTarget = Vector3.Distance(enemyManager.curTarget.transform.position, transform.position);

        int maxScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maxDistanceNeedToAttack && distanceFromTarget >= enemyAttackAction.minDistanceNeedToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxAttackAngle && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    maxScore += enemyAttackAction.attackScore;
                }
            }
        }

        int randomValue = Random.Range(0, maxScore);
        int tempScore = 0;

        for (int i = 0; i < enemyAttacks.Length; i++)
        {
            EnemyAttackAction enemyAttackAction = enemyAttacks[i];

            if (distanceFromTarget <= enemyAttackAction.maxDistanceNeedToAttack && distanceFromTarget >= enemyAttackAction.minDistanceNeedToAttack)
            {
                if (viewableAngle <= enemyAttackAction.maxAttackAngle && viewableAngle >= enemyAttackAction.minAttackAngle)
                {
                    if (curAttack != null)
                        return;

                    tempScore += enemyAttackAction.attackScore;

                    if (tempScore > randomValue)
                    {
                        curAttack = enemyAttackAction;
                    }
                }
            }
        }
    }

    public void HandleRotateTowardsTarger(EnemyManager enemyManager) //攻击始终朝着目标方向
    {
        if (enemyManager.isPreformingAction)
        {
            Vector3 direction = enemyManager.curTarget.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
        }
        //Roate with pathfinding
        else
        {
            Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
            Vector3 targetVelocity = enemyManager.enemyRig.velocity;

            enemyManager.navMeshAgent.enabled = true;
            enemyManager.navMeshAgent.SetDestination(enemyManager.curTarget.transform.position);
            enemyManager.enemyRig.velocity = targetVelocity;
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
