using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocmotion playerLocmotion;

    public bool isInteracting;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = CameraManager.singleton;
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
    }

}
