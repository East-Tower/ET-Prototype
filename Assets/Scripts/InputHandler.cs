using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    public bool roll_Input;
    public bool sprint_Input;
    public bool jump_Input;

    public bool roll_flag;
    public bool sprint_flag;
    public bool isInteracting;

    PlayerControls inputActions;
    PlayerLocomotion playerLocomotion;
    public CameraHandler cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    private void Awake()
    {
        cameraHandler = CameraHandler.singleton;
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void FixedUpdate()
    {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null) 
        {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, mouseX, mouseY);
        }
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput(float delta)
    {
        MoveInput(delta);
        HandleRollInput(delta);
        HandleSprintInput(delta);
        HandleJumpInput(delta);
    }

    private void MoveInput(float delta)
    {
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }

    private void HandleRollInput(float delta) 
    {
        roll_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (roll_Input) 
        {
            roll_flag = true;
        }
    }

    private void HandleSprintInput(float delta)
    {
        sprint_Input = inputActions.PlayerActions.Sprint.phase == UnityEngine.InputSystem.InputActionPhase.Started;

        if (sprint_Input && moveAmount !=0)
        {
            sprint_flag = true;
        }
    }

    private void HandleJumpInput(float delta) 
    {
        if (jump_Input) 
        {
            jump_Input = false;
            playerLocomotion.HandleJumping(delta);
        }
    }

}
