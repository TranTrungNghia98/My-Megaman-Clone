using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCutter : Enemy
{
    private Rigidbody2D supperCutterRb;
    private Vector3 playerPosition;
    private float jumpForce = 10.0f;
    private float moveSpeed = 10.0f;
    private float selfDestructTime = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        supperCutterRb = GetComponent<Rigidbody2D>();
        Jump();
        StartCoroutine(SelfDestruct());
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }


    void Jump()
    {
        playerPosition = GameObject.Find("Player").transform.position;
        supperCutterRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void MoveToPlayer()
    {
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        supperCutterRb.velocity = new Vector2(moveDirection.x * moveSpeed, supperCutterRb.velocity.y);
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }

}
