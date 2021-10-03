using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MainAnimatorManager
{
    EnemyLocomotion enemyLocomotion;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
    }

    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemyLocomotion.enemyRig.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyLocomotion.enemyRig.velocity = velocity;
    }
}
