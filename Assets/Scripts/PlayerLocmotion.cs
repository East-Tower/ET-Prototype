using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocmotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;
    Animator animator;

    public Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rig;

    [Header("下落")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;


    [Header("移动参数")]
    public float movementSpeed = 7;
    public float sprintSpeed = 10;
    public float rotationSpeed = 15;
    public float fallMovementSpeed = 1.5f;

    public float curSpeed;

    [Header("运动状态")]
    public bool isSprinting;
    public bool isFalling;
    public bool isGround;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        rig = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement() 
    {
        HandleFallingAndLanding();

        //互动状态下无法移动和转向(例外: 下落中)
        if (playerManager.isInteracting && !isFalling)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() 
    {
        //移动方向取决于相机的正面方向
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;


        curSpeed = movementSpeed;

        if (isSprinting)
        {
            curSpeed = sprintSpeed;
            moveDirection *= curSpeed;
        }
        else 
        {
            if (isFalling)
            {
                curSpeed = fallMovementSpeed;
                moveDirection *= curSpeed;
            }
            else 
            {
                moveDirection *= curSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;
        rig.velocity = movementVelocity;

        //if(isFalling) //下落时速度逐渐增加
        //{
        //    inAirTimer += Time.deltaTime;
        //    rig.AddForce(transform.forward * leapingVelocity);
        //    rig.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        //}
    }

    private void HandleRotation() 
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection += cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotataion = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotataion;
    }

    private void HandleFallingAndLanding() 
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += rayCastHeightOffset;

        //下落检测
        if (!isGround) 
        {
            if (!playerManager.isInteracting) 
            {
                animatorManager.PlayTargetAnimation("Falling", true);
                isFalling = true;
            }
        }

        //落地检测
        if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if (!isGround)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

            isFalling = false;
            inAirTimer = 0;
            isGround = true;
        }
        else 
        {
            isGround = false;
        }
    }
}
