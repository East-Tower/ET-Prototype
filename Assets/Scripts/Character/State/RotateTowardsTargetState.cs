using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTargetState : State
{
    public CombatStanceState combatStanceState;
    public float viewableAngle;

    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.animator.SetFloat("Vertical", 0);
        enemyAnimatorManager.animator.SetFloat("Horizontal", 0);

        Vector3 targetDirection = enemyManager.curTarget.transform.position - enemyManager.transform.position;
        viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

        if (enemyManager.isInteracting) 
        {
            return combatStanceState;
        }

        if (viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
        {
            Debug.Log("123");
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn180", true);
            return combatStanceState;
        }
        else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInteracting)
        {
            Debug.Log("321");
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn180", true);
            return combatStanceState;
        }
        else if (viewableAngle <= -25 && viewableAngle >= -100 && !enemyManager.isInteracting)
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnRight90", true);
            return combatStanceState;
        }
        else if (viewableAngle >= 25 && viewableAngle <= 100 && !enemyManager.isInteracting) 
        {
            enemyAnimatorManager.PlayTargetAnimationWithRootRotation("TurnLeft90", true);
            return combatStanceState;
        }


        return combatStanceState;
    }
}
