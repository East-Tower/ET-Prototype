using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotion enemyLocomotion;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public Rigidbody enemyRig;
    public NavMeshAgent navMeshAgent;
    public State curState;
    public CharacterStats curTarget;

    public bool isPreformingAction;
    public bool isInteracting;
    public float rotationSpeed = 15;
    public float maxAttackRange = 1.5f;

    [Header("AI Setting")]
    public float detectionRadius = 20;

    public float maxDetectionAngle = 50;
    public float minDetectionAngle = -50;

    public float curRecoveryTime = 0;
    private void Awake()
    {
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyStats = GetComponent<EnemyStats>();
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        enemyRig = GetComponent<Rigidbody>();
        navMeshAgent.enabled = false;
    }

    private void Start()
    {
        enemyRig.isKinematic = false;
    }
    private void Update()
    {
        HandleRecoveryTimer();
    }
    private void FixedUpdate()
    {
        HandleStateMachine();      
    }
    private void LateUpdate()
    {
        isInteracting = enemyAnimatorManager.animator.GetBool("isInteracting");
    }
    private void HandleStateMachine() 
    {
        if (curState != null && !isDead) 
        {
            State nextState = curState.Tick(this, enemyStats, enemyAnimatorManager);

            if (nextState != null) 
            {
                SwitchToNextState(nextState);
            }
        }
    }


    private void SwitchToNextState(State state) 
    {
        curState = state;
    }

    private void HandleRecoveryTimer() 
    {
        if (curRecoveryTime > 0) 
        {
            curRecoveryTime -= Time.deltaTime;
        }

        if (isPreformingAction) 
        {
            if (curRecoveryTime <= 0) 
            {
                isPreformingAction = false;
            }
        }
    }
}
