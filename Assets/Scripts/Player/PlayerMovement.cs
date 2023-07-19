using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRb;

    private float jumpForce = 600.0f;
    private float moveSpeed = 7.0f;
    private float horizontalInput;
    private float verticalInput;

    private bool isFaceRight = true;
    private bool isColldingAStair = false;
    private bool isClimbingStair = false;
    private bool isOnGround = false;
    private float startGravity;

    private TilemapCollider2D topLadderCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        startGravity = playerRb.gravityScale;
    }

    private void FixedUpdate()
    {
        // Player can move around with arrow keys
        Movement();

        // Press S to Jump
        if (Input.GetKeyDown(KeyCode.S) && isOnGround)
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
        }

        // Press Arrow Key Down and on top ladder. Set Top Ladder is trigger to player can climb down the ladder
        else if (Input.GetKey(KeyCode.DownArrow) && topLadderCollider != null)
        {
            topLadderCollider.isTrigger = true;
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
                // Prevent player falling down when is climbing but not move
                if (verticalInput == 0)
                {
                    playerRb.gravityScale = 0;
                }

                else
                {
                    ResetGravity();
                }

                playerRb.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);
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

    void ResetGravity()
    {
        playerRb.gravityScale = startGravity;
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
        isOnGround = false;
        playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
            isColldingAStair = false;
            ResetGravity();
            isClimbingStair = false;
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
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            isClimbingStair = false;
            ResetGravity();
        }

        else if (collision.gameObject.CompareTag("Top Ladder"))
        {
            isOnGround = true;
            topLadderCollider = collision.gameObject.GetComponent<TilemapCollider2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }
}
