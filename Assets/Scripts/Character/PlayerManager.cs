using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocmotion playerLocmotion;


    [Header("运动状态")]
    public bool isInteracting;
    public bool isUsingRootMotion;

    public bool isFalling;
    public bool isGround;
    public bool isSprinting;
    public bool isRolling;
    public bool isJumping;

   

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
        isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGround", isGround);
    }

}
