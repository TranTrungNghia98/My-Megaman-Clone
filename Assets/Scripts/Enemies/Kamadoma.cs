using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamadoma : Enemy
{
    // Movement Variable
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 checkGroundBoxSize;
    [SerializeField] private float checkGroundBoxDistance;
    [SerializeField] private LayerMask groundMask;
    private GameObject player;
    private Rigidbody2D kamadomaRb;

    // Sound Effect
    private AudioSource audioSource;
    [SerializeField] AudioClip jumpSound;

    // Animation
    private Animator kamadomaAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        kamadomaRb = GetComponent<Rigidbody2D>();
        kamadomaAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Jump when on ground
        if (isGrounded())
        {
            kamadomaAnimator.Play("Idle");
            // Animation
            LookAtPlayer();
            Jump();
        }
        // Move forward when on the air
        else
        {
            MoveForward();
        }
    }

    // Create check ground box
    // ABSTRACTION
    bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, checkGroundBoxSize, 0, -transform.up, checkGroundBoxDistance, groundMask))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    // Draw check ground box
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position - transform.up * checkGroundBoxDistance, checkGroundBoxSize);
    }

    void Jump()
    {
        // Animation
        kamadomaAnimator.Play("Jump");
        // Sound Effect
        audioSource.PlayOneShot(jumpSound);
        // Jump
        kamadomaRb.velocity = Vector2.up * jumpForce;
    }

    void MoveForward()
    {
        // Move to Player
        kamadomaRb.velocity = transform.right * moveSpeed + new Vector3(0, kamadomaRb.velocity.y);
    }

    void LookAtPlayer()
    {
        // If player tranform position x < tranform position x. It's mean player is on the left. So Turn Left
        if (player.transform.position.x < transform.position.x)
        {
            // Change rotation y = 180. To make game object turn Left
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        // Else player is on the Right. So Turn Right
        else if (player.transform.position.x > transform.position.x)
        {
            // Change rotation y = 0. To make game object turn Right
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
