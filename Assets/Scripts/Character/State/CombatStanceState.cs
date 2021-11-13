using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public AttackState attackState;
    public PursueState pursueState;

    public EnemyAttackAction[] enemyAttacks;


    bool randomDestinationSet = false;
    float verticalMovementVaule = 0;
    float horizontalMovementVaule = 0;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        //确认单位与目标间的距离
        float distanceFromTarget = Vector3.Distance(enemyManager.curTarget.transform.position, enemyManager.transform.position);
        enemyAnimatorManager.animator.SetFloat("Vertical", verticalMovementVaule, 0.2f, Time.deltaTime);
        enemyAnimatorManager.animator.SetFloat("Horizontal", horizontalMovementVaule, 0.2f, Time.deltaTime);
        attackState.hasPerformedAttack = false;

        if (enemyManager.isInteracting) 
        {
            //enemyAnimatorManager.animator.SetFloat("Vertical", 0);
            //enemyAnimatorManager.animator.SetFloat("Horizontal", 0);
            return this;
        }
        if (distanceFromTarget > enemyManager.maxAttackRange)
        {
            return pursueState; //距离大于攻击范围后退回追踪状态
        }

        if (!randomDestinationSet)
        {
            randomDestinationSet = true;
            //DecideCirclingAction(enemyAnimatorManager);
        }

        HandleRotateTowardsTarger(enemyManager); //保持面对目标的朝向

        if (enemyManager.curRecoveryTime <= 0 && attackState.curAttack!=null)
        {
            //randomDestinationSet = false;
            return attackState; //距离小于攻击范围且攻击间隔完成后进入攻击状态
        }
        else 
        {
            GetNewAttack(enemyManager);
            return this;
        }

    }

    public void HandleRotateTowardsTarger(EnemyManager enemyManager)
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

    private void DecideCirclingAction(EnemyAnimatorManager enemyAnimator) 
    {
        WalkAroundTarget(enemyAnimator);
    }
    private void WalkAroundTarget(EnemyAnimatorManager enemyAnimator) 
    {
        verticalMovementVaule = 0.5f;

        horizontalMovementVaule = Random.Range(-1, 1);

        if (horizontalMovementVaule <= 1 && horizontalMovementVaule >= 0)
        {
            horizontalMovementVaule = 0.5f;
        }
        else if (horizontalMovementVaule >= -1 && horizontalMovementVaule < 0)
        {
            horizontalMovementVaule = -0.5f;
        }
    }
    private void GetNewAttack(EnemyManager enemyManager) //攻击从设置好的攻击列表中随机挑选下一次的攻击动画(近战)
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
                    if (attackState.curAttack != null)
                        return;

                    tempScore += enemyAttackAction.attackScore;

                    if (tempScore > randomValue)
                    {
                        attackState.curAttack = enemyAttackAction;
                    }
                }
            }
        }
    }

}
