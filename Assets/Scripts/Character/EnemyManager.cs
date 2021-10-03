using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : CharacterManager
{
    EnemyLocomotion enemyLocomotion;

    public bool isPreformingAction;

    [Header("AI Setting")]
    public float detectionRadius = 20;

    public float maxDetectionAngle = 50;
    public float minDetectionAngle = -50;

    private void Awake()
    {
        enemyLocomotion = GetComponent<EnemyLocomotion>();
    }
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        HandleCurrentAction();

        
    }
    private void HandleCurrentAction() 
    {
        if (enemyLocomotion.curTarget == null)
        {
            enemyLocomotion.HandleDetection();
        }
        else 
        {
            enemyLocomotion.HandleMoveToTarget();
        }
    }
}
