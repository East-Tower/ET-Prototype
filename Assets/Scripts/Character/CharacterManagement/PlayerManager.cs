using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    //PlayerManager统一管理所有当前所处的状态, 与locomotion, input, camera的update
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocmotion playerLocmotion;
    PlayerStats playerStats;

    [Header("运动状态")]
    public bool isInteracting;
    public bool isUsingRootMotion;

    public bool isFalling; //下落时
    public bool isGround; //在地面时
    public bool isSprinting; 
    public bool isRolling;
    public bool isJumping; //跳跃上升阶段

    //战斗
    public bool isHitting;
    public bool isAttacking;
    //蓄力攻击相关
    public bool isCharging;
    public bool isAttackDashing;

    private void Awake()
    {
        Debug.Log("Awake第一");
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocmotion = GetComponent<PlayerLocmotion>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        Debug.Log("Start第一");
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        playerStats.StaminaRegen();
    }

    private void FixedUpdate()
    {
        playerLocmotion.HandleAllMovement();
        cameraManager.HandleAllCameraMovement();
    }

    private void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");
        isUsingRootMotion = animator.GetBool("isUsingRootMotion");
        isCharging = animator.GetBool("isCharging");
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isGround", isGround); 
        animator.SetBool("isFalling", isFalling);

        inputManager.reAttack_Input = false;
        if (!isCharging) 
        {
            inputManager.spAttack_Input = false;
        }
    }
}
