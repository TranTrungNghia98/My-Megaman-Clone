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

    void Jump()
    {
        // Jump
        kamadomaRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isOnGround = false;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            // Look at player then jump to player
            LookAtPlayer();
            Jump();
        }
    }

}
