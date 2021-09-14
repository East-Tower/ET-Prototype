using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    AnimatorManager animatorManager;
    PlayerLocmotion playerLocmotion;

    Vector2 movementInput;
    Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool sprint_Input; //跑步键

    private void Awake()
    {
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
    }

    private void HandleMovement() 
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorVaules(0, moveAmount, playerLocmotion.isSprinting);
    }

    private void HandleSprintInput()
    {
        sprint_Input = playerControls.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (sprint_Input && moveAmount != 0)
        {
            playerLocmotion.isSprinting = true;
        }
        else 
        {
            playerLocmotion.isSprinting = false;
        }
    }
}
