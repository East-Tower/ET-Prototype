using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocmotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    AnimatorManager animatorManager;

    Vector3 moveDirection;
    Transform cameraObject;
    public Rigidbody rig;

    [Header("下落")]
    [SerializeField] float inAirTimer;
    [SerializeField] float leapingVelocity;
    [SerializeField] float fallingVelocity;
    [SerializeField] float rayCastHeightOffset;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float radius;

    public Vector3 rayCastOrigin; //temp

    [Header("移动参数")]
    [SerializeField] float movementSpeed = 7;
    [SerializeField] float sprintSpeed = 10;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float fallMovementSpeed = 1.5f;

    [Header("跳跃参数")]
    [SerializeField] float jumpHeight = 3;
    [SerializeField] float gravityIntensity = -15;

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
        if (playerManager.isInteracting)
            return;

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() 
    {
        if (playerManager.isJumping)
            return;

        if (playerManager.isFalling)
            return;

        //移动方向取决于相机的正面方向
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float curSpeed = movementSpeed;

        if (playerManager.isSprinting)
        {
            curSpeed = sprintSpeed;
            moveDirection *= curSpeed;
        }
        else 
        {
            if (playerManager.isFalling)
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
        rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y += rayCastHeightOffset;
        targetPosition = transform.position;

        //下落检测(当前下落无法移动)
        if (!playerManager.isGround && !playerManager.isJumping) 
        {
            if (!playerManager.isInteracting) 
            {
                animatorManager.PlayTargetAnimation("Falling", true);
                playerManager.isFalling = true;
            }

            animatorManager.animator.SetBool("isUsingRootMotion", false);
            inAirTimer += Time.deltaTime;
            rig.AddForce(transform.forward * leapingVelocity); //x轴初速度
            rig.AddForce(-Vector3.up * fallingVelocity * inAirTimer); //y轴重力加速
        }

        //落地检测
        if (Physics.SphereCast(rayCastOrigin, radius, -Vector3.up, out hit, groundLayer))
        {
            if (!playerManager.isGround)
            {
                animatorManager.PlayTargetAnimation("Land", true);
                rig.velocity = new Vector3(0, rig.velocity.y, 0);

            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTimer = 0;
            playerManager.isGround = true;
            playerManager.isFalling = false;
        }
        else 
        {
            playerManager.isGround = false;
        }

        if (playerManager.isGround && !playerManager.isJumping) 
        {
            if (playerManager.isInteracting || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else 
            {
                transform.position = targetPosition;
            }
        }
    }
    public void HandleJumping() 
    {
        if (playerManager.isGround) 
        {
            animatorManager.animator.SetBool("isJumping", true);

            if (inputManager.moveAmount != 0)
            {
                animatorManager.PlayTargetAnimation("JumpMove", true);
            }
            else 
            {
                animatorManager.PlayTargetAnimation("Jump", true);
            }

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocty = moveDirection;
            playerVelocty.y = jumpingVelocity;
            rig.velocity = playerVelocty;
        }
    }
    public void HandleRoll() 
    {
        if (playerManager.isInteracting)
            return;

        animatorManager.PlayTargetAnimation("Rolling", true, true);
        //Toggle Invulnerable Bool  for no damage during animation
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(new Vector3(rayCastOrigin.x, rayCastOrigin.y + rayCastHeightOffset, rayCastOrigin.z), radius);
    //}
}
