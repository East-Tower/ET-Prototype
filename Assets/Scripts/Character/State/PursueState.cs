using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueState : State
{
    public IdleState idleState;
    public CombatStanceState combatStanceState;
    public RotateTowardsTargetState rotateTowardsTargetState;

    public float distanceFromTarget;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        Vector3 targetDirection = enemyManager.curTarget.transform.position - enemyManager.transform.position;
        distanceFromTarget = Vector3.Distance(enemyManager.curTarget.transform.position, enemyManager.transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);
        HandleRotateTowardsTarger(enemyManager);

        if (viewableAngle > 65 || viewableAngle < -65)
            return rotateTowardsTargetState;

        if (enemyManager.isPreformingAction) 
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0,0.1f, Time.deltaTime);
            return this;
        }            

        if (distanceFromTarget > enemyManager.maxAttackRange)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);   //朝着目标单位进行移动
        }
        else if (distanceFromTarget <= enemyManager.maxAttackRange)
        {
            enemyAnimatorManager.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);   //站着idle状态
        }

        enemyManager.navMeshAgent.transform.localPosition = Vector3.zero;
        enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

        if (distanceFromTarget <= enemyManager.maxAttackRange)
        {
            return combatStanceState;
        }
        else if (distanceFromTarget >= enemyManager.pursueMaxDistance)
        {
            enemyManager.curTarget = null;
            return idleState;
        }
        else 
        {
            return this;
        }
    }

    public void HandleRotateTowardsTarger(EnemyManager enemyManager) //追踪时保持朝着目标方向
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
