using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator playerAnimator;
    private string currentAnimation;

    // ANIMATION NAMES
    //private const string IDLE = "Idle";
    //private const string IDLE_SHOOT = "Idle Shoot";
    //private const string RUN = "Run";
    //private const string RUN_SHOOT = "Run Shoot";
    //private const string CLIMB = "Climb";
    //private const string JUMP = "Jump";
    //private const string JUMP_SHOOT = "Jump Shoot";

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ABSTRACTION
    public void PlayAnimation(string animationName)
    {
        if (currentAnimation == animationName)
        {
            return;
        }

        playerAnimator.Play(animationName);

        currentAnimation = animationName;
    }
}
