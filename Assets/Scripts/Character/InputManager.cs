using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocmotion playerLocmotion;

    Vector2 movementInput;
    Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    bool sprint_Input; //跑步键
    bool roll_Input; //翻滚/冲刺键
    public bool jump_Input; //跳跃

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocmotion = GetComponent<PlayerLocmotion>();
    }

    private void OnEnable()
    {
        if (playerControls == null) 
        {
            playerControls = new PlayerControls();

            //设置input的输入
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            //判定是否有跳跃输入
            playerControls.PlayerActions.Jump.started += i => jump_Input = true;
            playerControls.PlayerActions.Jump.canceled += i => jump_Input = false;

            playerControls.PlayerActions.Roll.performed += i => roll_Input = true;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs() 
    {
        HandleMovement();
        HandleSprintInput();
        HandleRollInput();
    }

    private void HandleMovement() 
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorVaules(0, moveAmount, playerManager.isSprinting);
    }

    private void HandleSprintInput()
    {
        sprint_Input = playerControls.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (sprint_Input && moveAmount != 0)
        {
            playerManager.isSprinting = true;
        }
        else 
        {
            playerManager.isSprinting = false;
        }
    }



    private void HandleRollInput() 
    {
        if (roll_Input) 
        {
            roll_Input = false;
            playerLocmotion.HandleRoll();
        }
    }
}
