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
    private float startTime = 2.0f;
    private float attackRate = 1.0f;

    // Movement variable
    private Rigidbody2D cutManRb;
    private float moveSpeed = 10.0f;
    private float jumpForece = 12.0f;
    private float maxDistance = 15.0f;
    private float minDistance = 10.0f;
    private float currentDistance;
    private bool isTurnRight = false;
    private bool isOnGround = true;
    // Start is called before the first frame update
    void Start()
    {
        cutManRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        // Only Attack when have rolling cutter and on the ground
        InvokeRepeating("ThrowRollingCutter", startTime, attackRate);
    }

    private void FixedUpdate()
    {
        // Move to player 
        if (currentDistance >= minDistance)
        {
            MoveToPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }


    // Movement Methods
    void CheckDistance()
    {
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
    }

    void Jump()
    {
        isOnGround = false;
        cutManRb.AddForce(Vector2.up * jumpForece, ForceMode2D.Impulse);
    }

    void MoveToPlayer()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        cutManRb.velocity = moveDirection * moveSpeed + new Vector3(0, cutManRb.velocity.y);
    }

    void LookAtPlayer()
    {
        float playerPosX = player.transform.position.x;

        // If player pos x < tranform.position.x. It means player is on the left of boss.
        // Check if boss turned left or not. If not turn left
        if (playerPosX < transform.position.x && isTurnRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // If player pos x > tranform.position.x. It means player is on the righ of boss.
        // Check if boss turned right or not. If not turn right
        else if (playerPosX > transform.position.x && !isTurnRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Attack Methods
    void ThrowRollingCutter()
    {
        if (haveRollingCutter && isOnGround)
        {
            Instantiate(rollingCutterPrefab, rollingCutterPosition.transform.position, transform.rotation);
            haveRollingCutter = false;
        }
    }

    public void PickUpRollingCutter()
    {
        haveRollingCutter = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Only look at player when stand on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

            LookAtPlayer();

            // If boss is too far player, jump to player
            if (currentDistance >= maxDistance)
            {
                Jump();
            }
        }
    }
}
