using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private PlayerAnimator playerAnimatorScript;
    private PlayerAttack playerAttackScript;
    private PlayerStats playerStatsScript;
    private PlayerSoundEffect playerSoundEffectScript;

    private float jumpForce = 8.0f;
    private float moveSpeed = 7.0f;
    private float climbSpeed = 5.0f;
    public float horizontalInput { get; private set; }
    private float verticalInput;

    private bool isFaceRight = true;
    private bool isColldingAStair = false;
    public bool isClimbingStair { get; private set; }
    private float startGravity;

    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float distance;
    [SerializeField] private LayerMask groundMask;

    private BoxCollider2D topLadderCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimatorScript = GetComponent<PlayerAnimator>();
        playerAttackScript = GetComponent<PlayerAttack>();
        playerStatsScript = GetComponent<PlayerStats>();
        playerSoundEffectScript = GetComponent<PlayerSoundEffect>();

        startGravity = playerRb.gravityScale;
    }

    private void FixedUpdate()
    {
        // Player can move around with arrow keys
        Movement();

        // Press S to Jump
        // PLayer can jump when on ground and isn't get hurted
        if (Input.GetKeyDown(KeyCode.S) && isGrounded() && !playerStatsScript.isHurting)
        {
            // Make sure player only can jump when player is on the ground
            Jump();
        }
    }

    private void Update()
    {
        // Press S to Dropdown from the stair
        if (Input.GetKeyDown(KeyCode.S))
        {
            // Make player can Dropdown when player is climbing on the stair
            isClimbingStair = false;
            // Reset Constraint to prevent player keep stading in the stair
            ResetGravity();
            playerAnimatorScript.PlayAnimation("Jump");
        }

        // Press Arrow Key Down and on top ladder. Set Top Ladder is trigger to player can climb down the ladder
        else if (Input.GetKey(KeyCode.DownArrow) && topLadderCollider != null)
        {
            topLadderCollider.isTrigger = true;
        }
    }

    private void LateUpdate()
    {
        // Animations
        // Only change animation when attack or hurt animation isn't playing
        // Run & Idle
        if (!playerStatsScript.isHurting && !playerAttackScript.isPlayingShootAnimation)
        {
            if (isGrounded())
            {
                if (horizontalInput == 0)
                {
                    playerAnimatorScript.PlayAnimation("Idle");
                }

                else
                {
                    playerAnimatorScript.PlayAnimation("Run");
                }
            }
            // Climb
            else if (isClimbingStair)
            {
                playerAnimatorScript.PlayAnimation("Climb");
            }
        }

    }

    // Movement
    void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check Face Direction To Turn Right Direction when moving
        CheckFaceDirection();

        // If Player is colleding with a Stair, player can move up
        if (isColldingAStair)
        {
            // Check if player is climbing the stair
            if (verticalInput != 0)
            {
                isClimbingStair = true;
            }
            // If player want to climbing or is climbing the stair, move player up and down.
            if (isClimbingStair)
            {
                // Turn off gravity to prevent gravity make player move down when climbing
                playerRb.gravityScale = 0;
                playerRb.velocity = new Vector2(horizontalInput * climbSpeed, verticalInput * climbSpeed);
            }

            // If player only colldie with the stair and don't want to climbing on it. So player can jump and fall when they collide with the stair
            else
            {
                playerRb.velocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);
            }
        }

        // If Player isn't colleding with a Stair, player can't move up
        else
        {
            playerRb.velocity = new Vector2(horizontalInput * moveSpeed, playerRb.velocity.y);
        }


    }

    public void ResetGravity()
    {
        isClimbingStair = false;
        playerRb.gravityScale = startGravity;
    }

    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, distance, groundMask))
        {
            isClimbingStair = false;
            return true;
        }

        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position - transform.up * distance, boxSize);
    }

    // Rotate Face
    void CheckFaceDirection()
    {
        // If player is facing to the right and player move to the right. Turn player face to the right
        if (!isFaceRight && horizontalInput > 0)
        {
            isFaceRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        // Ì player is facing to the right and player move to the left. Turn player face to the left
        else if (isFaceRight && horizontalInput < 0)
        {
            isFaceRight = false;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // Jump
    void Jump()
    {
        playerRb.velocity = Vector2.up * jumpForce;
        isClimbingStair = false;

        //  Animation
        if (!playerAttackScript.isPlayingShootAnimation)
        {
            playerAnimatorScript.PlayAnimation("Jump");
        }

        // Sound Effect
        playerSoundEffectScript.PlaySoundEffect("Jump");
    }

    // Collide with Stair , Ground
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            isColldingAStair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stair"))
        {
            ResetGravity();
            isColldingAStair = false;
            isClimbingStair = false;
            playerAnimatorScript.PlayAnimation("Idle");
        }


        else if (collision.gameObject.CompareTag("Top Ladder"))
        {
            // Turn off trigger in top ladder so player can stand on it
            if (topLadderCollider.isTrigger)
            {
                topLadderCollider.isTrigger = false;
            }
            // Turn off collider when player not on top ladder
            topLadderCollider = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Top Ladder"))
        {
            topLadderCollider = collision.gameObject.GetComponent<BoxCollider2D>();
        }
    }
}
