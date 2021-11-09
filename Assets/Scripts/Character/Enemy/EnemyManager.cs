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
    public float rotationSpeed = 0.8f;
    public float maxAttackRange = 3f;
    public float moveSpeed=1f;

    [Header("AI Setting")]
    public float detectionRadius = 10;
    public float pursueMaxDistance = 12;

    public float maxDetectionAngle = 70;
    public float minDetectionAngle = -70;

    public float curRecoveryTime = 0;
    public bool canDoCombo;

    public FlyingObj arrow;
    public Transform shootPos;
    public Transform target;

    //BossOnly
    public int curTargetAngle; //0 - Front, 1 - Back, 2 - Flank
    public int curTargetDistance; //0 - Short, 1 - Mid, 2 - Long

    public float distanceFromTarget;
    public float viewableAngle;

    public float shoutRadius;
    public LayerMask playerLayer;

    [SerializeField] float mediumRange = 6f;

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
            maxAttackRange = 3f;
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
        HandleTargetPositionCheck();
        isRotatingWithRootMotion = enemyAnimatorManager.animator.GetBool("isRotatingWithRootMotion");
        canRotate = enemyAnimatorManager.animator.GetBool("canRotate");

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

    private void HandleTargetPositionCheck() 
    {
        Vector3 targetDirection = curTarget.transform.position - transform.position;
        distanceFromTarget = Vector3.Distance(curTarget.transform.position, transform.position);
        float viewableAngle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up);

        //Angle Check
        if (viewableAngle >= 0 && viewableAngle < 45)
        {
            curTargetAngle = 0;
        }
        else if (viewableAngle < 0 && viewableAngle > -45)
        {
            curTargetAngle = 0;
        }
        else if (viewableAngle >= 45 && viewableAngle < 110) 
        {
            curTargetAngle = 2;
        }
        else if (viewableAngle <= -45 && viewableAngle > -110)
        {
            curTargetAngle = 2;
        }
        else if (viewableAngle >= 110 && viewableAngle <= 180)
        {
            curTargetAngle = 1;
        }
        else if (viewableAngle <= -110 && viewableAngle >= -180)
        {
            curTargetAngle = 1;
        }

        //Distance Check
        if (distanceFromTarget > 0 && distanceFromTarget <= maxAttackRange)
        {
            curTargetDistance = 0;
        }
        else if (distanceFromTarget > maxAttackRange && distanceFromTarget <= mediumRange)
        {
            curTargetDistance = 1;
        }
        else if (distanceFromTarget > mediumRange)
        {
            curTargetDistance = 2;
        }
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

    public void HandleRangeAttack() 
    {
        var obj = Instantiate(arrow, transform, false);
        obj.transform.SetParent(null);
        obj.gameObject.SetActive(true);
        obj.StartFlyingObj(target);
    }

    void OnDrawGizmosSelected()
    {
         Gizmos.color = Color.blue;
         Gizmos.DrawWireSphere(transform.position, shoutRadius);
    }
}
