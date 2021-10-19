using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerStats playerStats;
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocmotion playerLocmotion;
    PlayerAttacker playerAttacker;
    PlayerInventory playerInventory;

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

    //攻击
    public bool reAttack_Input;
    public bool spAttack_Input;
    public float chargingTimer;
    public bool charged_Input;

    //锁定
    CameraManager cameraManager;
    public bool lockOn_Input;
    public bool lockOn_Flag;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerStats = GetComponent<PlayerStats>();
        animatorManager = GetComponentInChildren<AnimatorManager>();
        playerLocmotion = GetComponent<PlayerLocmotion>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerInventory = GetComponent<PlayerInventory>();
        cameraManager = FindObjectOfType<CameraManager>();
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

            //攻击输入
            playerControls.PlayerActions.RegularAttack.performed += i => reAttack_Input = true;
            playerControls.PlayerActions.SpecialAttack.performed += i => spAttack_Input = true;
            playerControls.PlayerActions.SpecialAttack.canceled += i => spAttack_Input = false;

            //锁定模式
            playerControls.PlayerActions.LockOn.performed += i => lockOn_Input = true;
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
        HandleAttackInput();
        HandleLockOnInput();
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

        if (sprint_Input && moveAmount != 0 && playerStats.currStamina > 0)
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
    private void HandleAttackInput() 
    {
        if (reAttack_Input) 
        {
            playerAttacker.HandleRegularAttack(playerInventory.equippedItem);
        }

        if (spAttack_Input)
        {
            playerAttacker.HandleSpecialAttack(playerInventory.equippedItem);
        }
        else 
        {
            playerManager.isCharging = false;
        }
    }
    private void HandleLockOnInput() 
    {
        if (lockOn_Input && !lockOn_Flag) 
        {
            cameraManager.ClearLockOnTargets();
            lockOn_Input = false;
            cameraManager.HandleLockOn();
            if (cameraManager.nearestLockOnTarget != null) 
            {
                cameraManager.currentLockOnTarget = cameraManager.nearestLockOnTarget;
                lockOn_Flag = true;
            }
        }
        else if(lockOn_Input && lockOn_Flag)
        {
            lockOn_Input = false;
            lockOn_Flag = false;
            //取消锁定
            cameraManager.ClearLockOnTargets();
        }
    }
}
