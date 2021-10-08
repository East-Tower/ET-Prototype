﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocmotion playerLocmotion;


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
        cameraManager = FindObjectOfType<CameraManager>();
        animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        playerLocmotion = GetComponent<PlayerLocmotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
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