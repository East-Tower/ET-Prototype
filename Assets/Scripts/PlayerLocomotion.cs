using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform myTransform;
    [HideInInspector]
    public AnimatorHandler animatorHandler;

    public new Rigidbody rigibody;
    public GameObject normalCamera;

    [Header("Stats")]
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float rotationSpeed = 15f;

    [Header("Jump")]
    public float gravityIntensity = -15;
    public float jumpHeight = 3;

    public bool isSprinting;
    public bool isJumping;


    void Start()
    {
        rigibody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        cameraObject = Camera.main.transform;
        myTransform = transform;
        animatorHandler.Initialize();
    }

    public void Update()
    {
        float delta = Time.deltaTime;
        inputHandler.TickInput(delta);
        HandleMovement(delta);
        HandleRollingAndSprinting(delta);
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandlerRotation(float delta)
    {
        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right*inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
            targetDir = myTransform.forward;

        float rs = rotationSpeed;

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }

    public void HandleMovement(float delta) 
    {
        //在滚动过程中不能随便移动
        if (animatorHandler.anim.GetBool("isInteracting"))
            return;

        if (isJumping)
            return;

        //朝摄像机相对方向移动
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float speed = movementSpeed;

        if (inputHandler.sprint_flag)
        {
            speed = sprintSpeed;
            isSprinting = true;
            moveDirection *= speed;
        }
        else 
        {
            isSprinting = false;
            moveDirection *= speed;
        }

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigibody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0, isSprinting);

        if (animatorHandler.canRotate)
        {
            HandlerRotation(delta);
        }
    }

    public void HandleRollingAndSprinting(float delta) 
    {
        if (animatorHandler.anim.GetBool("isInteracting"))
            return;

        if (inputHandler.roll_flag) 
        {
            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;

            if (inputHandler.moveAmount > 0)
            {
                animatorHandler.PlayTargetAnimation("Rolling", true);
                moveDirection.y = 0;
                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                myTransform.rotation = rollRotation;
            }
            else 
            {
                //原地滚, 朝角色面朝方向滚
                animatorHandler.PlayTargetAnimation("Rolling", true);

                Quaternion rollRotation = Quaternion.LookRotation(transform.forward);
                myTransform.rotation = rollRotation;
            }
        }
    }

    #endregion

    public void HandleJumping(float delta) 
    {
        animatorHandler.anim.SetBool("isJumping", true);
        animatorHandler.PlayTargetAnimation("Jump", false);

        float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
        Vector3 playerVelocity = moveDirection;
        playerVelocity.y = jumpingVelocity;
        rigibody.velocity = playerVelocity;
    }
}
