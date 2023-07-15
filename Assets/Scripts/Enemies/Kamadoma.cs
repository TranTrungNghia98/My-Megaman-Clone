using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamadoma : Enemy
{
    // Start is called before the first frame update
    private float jumpForce = 6.0f;
    private float moveSpeed = 4.0f;
    private GameObject player;
    private Rigidbody2D kamadomaRb;
    private bool isOnGround = false;
    void Start()
    {
        player = GameObject.Find("Player");
        kamadomaRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isOnGround)
        {
            MoveForward();
        }
    }

    private void Update()
    {
        // Look At Player The Jump Forward
        LookAtPlayer();
    }

    void Jump()
    {
        // Jump
        kamadomaRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isOnGround = false;
        // Reset Constraints so game object can Jump
        ResetConstraints();
    }

    void MoveForward()
    {
        // Move to Player
        kamadomaRb.AddForce(transform.right * moveSpeed);
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

    void ResetConstraints()
    {
        kamadomaRb.constraints = RigidbodyConstraints2D.None;
        kamadomaRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            // Prevent game object fall down the ground then Jump
            kamadomaRb.constraints = RigidbodyConstraints2D.FreezePositionY;
            Jump();
        }

        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            Destroy(collision.gameObject);
            // Get Damage when has been shoot
            GetDamage();
            // Check if enemy alive
            CheckHealth();
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            playerStatsScript = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStatsScript.GetDamage(damage);
        }
    }
}
