using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : CharacterManager
{
    EnemyLocomotion enemyLocomotion;
    EnemyAnimatorManager enemyAnimatorManager;
    EnemyStats enemyStats;

    public Rigidbody enemyRig;
    public NavMeshAgent navMeshAgent;
    public State curState;
    public CharacterStats curTarget;

    public enum enemyType {melee, range};
    public enemyType curEnemyType;

    //木桩
    public bool isDummy;

    //待机模式
    public enum IdleType {Stay, Patrol};
    public IdleType idleType;
    public List<Transform> patrolPos = new List<Transform>();
    public int curPatrolIndex = 0;

    public bool isPreformingAction;
    public bool isInteracting;
    public bool isImmuneAttacking;
    public float rotationSpeed = 15;
    public float maxAttackRange = 1.5f;

    [Header("AI Setting")]
    public float detectionRadius = 10;
    public float pursueMaxDistance = 12;

    public float maxDetectionAngle = 70;
    public float minDetectionAngle = -70;

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
        foreach (Transform child in gameObject.transform.parent)
        {
            if (child.tag != "Enemy")
            {
                patrolPos.Add(child);
            }
        }

        if (curEnemyType == enemyType.melee)
        {
            maxAttackRange = 1.5f;
        }
        else if (curEnemyType == enemyType.range) 
        {
            maxAttackRange = 15f;
            detectionRadius = 18f;
            pursueMaxDistance = 25f;
        }
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
    private void HandleStateMachine() //单位状态机管理
    {
        if (curState != null && !isDead && !isDummy) 
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

    private void HandleRecoveryTimer() //攻击间隔
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
