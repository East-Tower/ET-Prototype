using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    InputHandler inputHandler;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        inputHandler = GetComponent<InputHandler>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        inputHandler.isInteracting = anim.GetBool("isInteracting");
        playerLocomotion.isJumping = anim.GetBool("isJumping");
        inputHandler.roll_flag = false;
        inputHandler.sprint_flag = false;
    }
}
