using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MainAnimatorManager
{
    PlayerManager playerManager;
    PlayerLocmotion playerLocmotion;
    InputManager inputManager;
    AudioSource audio;
    int horizontal;
    int vertical;

    //VFX
    Sample_VFX sample_VFX;
    public Sample_SFX sample_SFX;

    public float animatorPlaySpeed = 1;

    public bool ifSpeedChanged;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerLocmotion = GetComponentInParent<PlayerLocmotion>();
        inputManager = GetComponentInParent<InputManager>();
        audio = GetComponent<AudioSource>();
        sample_VFX = FindObjectOfType<Sample_VFX>();
        sample_SFX = FindObjectOfType<Sample_SFX>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        animator.applyRootMotion = false;
    }
    public void UpdateAnimatorVaules(float horizontalMovement, float verticalMovement, bool isSprinting) 
    {
        //数值判断是走还是跑
        float v = 0;
        float h = 0;
        #region Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            v = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }
        #endregion
        #region Horizontal;
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }
        #endregion

        if (isSprinting) 
        {
            v = 2;
            h = horizontalMovement;
        }


        animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    } //Locomotion数值变化(如果有需要在locomotion中加入额外的状态在这里改)

    private void OnAnimatorMove()
    {
        if (playerManager.isUsingRootMotion)
        {
            if (playerManager.isGround)
            {
                playerLocmotion.rig.drag = 0;
                Vector3 deltaPosition = animator.deltaPosition;
                deltaPosition.y = 0;
                Vector3 velocity = deltaPosition / Time.deltaTime;

                if (playerManager.isHitting)
                {
                    playerLocmotion.rig.velocity = new Vector3(0, playerLocmotion.rig.velocity.y, 0);
                }
                else
                {
                    playerLocmotion.rig.velocity = velocity;
                    if (!playerManager.isGround)
                    {
                        playerLocmotion.rig.velocity = new Vector3(0, playerLocmotion.rig.velocity.y, 0);
                        playerLocmotion.HandleGravity();
                    }
                }
            }
            else
            {
                //想想办法
            }
        }
    }

    //Animator Events Editor  
    private void AnimatorPlaySpeed(float playRate) 
    {
        animator.speed = playRate;
    }

    private void AnimatorPlaySound(int clipNum)
    {
        audio.clip = sample_SFX.curSFX_List[clipNum];
        audio.Play();
    }

    private void AnimatorPlayVFX(int num)
    {
        sample_VFX.curVFX_List[num].Play();
    }

    private void AnimatorStop(int stopDuration)
    {
        StartCoroutine(Pause(stopDuration));
    }

    IEnumerator Pause(int dur)
    {
        float pauseTime = dur / 60f;
        animator.speed = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        animator.speed = 1;
    }

}