using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMan : Enemy
{
    private GameObject player;

    // Attack Variable
    [SerializeField] private GameObject rollingCutterPrefab;
    [SerializeField] private GameObject rollingCutterPosition;
    private bool haveRollingCutter = true;
    private float startTime = 1.5f;
    private float attackRate = 0.5f;

    // Get Damaged Variable
    public bool isHurting { get; private set; }
    private float hurtingTime;
    private bool isInvincible;
    private float invincibleTime = 1.0f;

    // Movement variable
    private Rigidbody2D cutManRb;
    private float moveSpeed = 7.0f;
    private float moveSpeedInAir = 3.0f;
    private float jumpForce = 5.0f;
    private float idleRange = 5.0f;
    private float jumpRange = 3.0f;
    private float currentDistance;
    private bool isTurnRight = false;

    // Check ground variable
    [SerializeField] private Vector3 checkGroundBoxSize;
    [SerializeField] private float checkGroundDistance;
    [SerializeField] private LayerMask groundMask;

    // Animation Variable
    private Animator cutmanAnimator;
    private bool isAttackAnimationPlaying;
    private float attackAnimationTime;

    // Sound Effect Variable
    private AudioSource audioSource;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        cutManRb = GetComponent<Rigidbody2D>();
        cutmanAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = GameObject.Find("Player");
        // Only Attack when have rolling cutter and on the ground
        InvokeRepeating("ThrowRollingCutter", startTime, attackRate);
    }

    private void FixedUpdate()
    {
        // Movement when On Ground
        if (IsGrounded())
        {
            // Move to player when player too far from boss & boss is on Grounded
            if (currentDistance > idleRange)
            {
                MoveToPlayer(moveSpeed);
            }

            // Jump to player when player too near from boss
            else if (currentDistance <= jumpRange)
            {
                Jump();
            }

            // Else . Stop Moving
            else
            {
                StopMoving();
            }
        }

        // Movement When on Air
        else
        {
            MoveToPlayer(moveSpeedInAir);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        LookAtPlayer();
    }

    // Movement Methods
    bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, checkGroundBoxSize, 0, -transform.up, checkGroundDistance, groundMask))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position - transform.up * checkGroundDistance, checkGroundBoxSize);
    }

    void CheckDistance()
    {
        currentDistance = Vector3.Distance(player.transform.position, transform.position);
    }

    void Jump()
    {
        // Play Animation when attack or huting animation isn't playing
        if (!isAttackAnimationPlaying && !isHurting)
        {
            PlayJumpAnimation();
            // Sound Effect
            audioSource.PlayOneShot(jumpSound);
        }

        // Logic
        cutManRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void MoveToPlayer(float speed)
    {
        // Play Animation when attack or huting animation isn't playing
        if (!isAttackAnimationPlaying && !isHurting)
        {
            PlayMoveToPlayerAnimation();
        }

        // Logic
        ContinueMoving();
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        cutManRb.velocity = new Vector3(moveDirection.x * speed, cutManRb.velocity.y);
    }

    void StopMoving()
    {
        // Play Animation when attack or huting animation isn't playing
        if (!isAttackAnimationPlaying && !isHurting)
        {
            PlayIdleAnimation();
        }

        cutManRb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    void ContinueMoving()
    {
        cutManRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void LookAtPlayer()
    {
        float playerPosX = player.transform.position.x;

        // If player pos x < tranform.position.x. It means player is on the left of boss.
        // Check if boss turned left or not. If not turn left
        if (playerPosX < transform.position.x && isTurnRight)
        {
            isTurnRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }

        // If player pos x > tranform.position.x. It means player is on the righ of boss.
        // Check if boss turned right or not. If not turn right
        else if (playerPosX > transform.position.x && !isTurnRight)
        {
            isTurnRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Attack Methods
    void ThrowRollingCutter()
    {
        // If boss have cutter, throw cutter
        if (haveRollingCutter && !isHurting)
        {
            // Play Animation when huting animation isn't playing
            PlayThrowCutterAnimation();
            // Logic
            haveRollingCutter = false;
            Instantiate(rollingCutterPrefab, rollingCutterPosition.transform.position, transform.rotation);
        }
    }

    public void PickUpRollingCutter()
    {
        haveRollingCutter = true;
    }

    // Get Damage Methods
    IEnumerator DeactiveInvincibleMode()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    protected override void GetDamage()
    {
        // Play Hurt Animation
        PlayHurtAnimation();
        // Play Sound Effect
        audioSource.PlayOneShot(hurtSound);
        // Get Damage
        base.GetDamage();
        // Turn on Invicible so boss won't get damage in a few seconds
        isInvincible = true;
        isHurting = true;
        // Turn off invicible so boss can get damage
        StartCoroutine(DeactiveInvincibleMode());
        // Change turnning hurt state to change to other animation
        StartCoroutine(TurnIsHurtingToFalse());
    }

    // Animation Methods
    void PlayJumpAnimation()
    {
        if (haveRollingCutter)
        {
            cutmanAnimator.Play("Jump");
        }

        else
        {
            cutmanAnimator.Play("Jump Without Cutter");
        }
    }

    void PlayMoveToPlayerAnimation()
    {
        if (haveRollingCutter)
        {
            cutmanAnimator.Play("Run");
        }

        else
        {
            cutmanAnimator.Play("Run Without Cutter");
        }
    }

    void PlayIdleAnimation()
    {
        if (haveRollingCutter)
        {
            cutmanAnimator.Play("Idle");
        }

        else
        {
            cutmanAnimator.Play("Idle Without Cutter");
        }
    }

    void PlayThrowCutterAnimation()
    {
        cutmanAnimator.Play("Throw Cutter");
        isAttackAnimationPlaying = true;
        attackAnimationTime = cutmanAnimator.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(StopAttackAnimation());
    }

    void PlayHurtAnimation()
    {
        cutmanAnimator.Play("Hurt");
    }

    IEnumerator StopAttackAnimation()
    {
        yield return new WaitForSeconds(attackAnimationTime);
        isAttackAnimationPlaying = false;
    }

    IEnumerator TurnIsHurtingToFalse()
    {
        yield return new WaitForSeconds(hurtingTime);
        isHurting = false;
    }

    // Rewrite OnTrigger Enter 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            if (!isInvincible)
            {
                Destroy(collision.gameObject);
                // Get Damage when has been shoot
                GetDamage();
                // Check if enemy alive
                CheckHealth();
            }
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            playerStatsScript = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStatsScript.GetDamage(damage);
        }
    }
}
