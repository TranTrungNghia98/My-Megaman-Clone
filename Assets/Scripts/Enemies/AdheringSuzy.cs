using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdheringSuzy : Enemy
{
    // Movement
    private float moveSpeed = 5.0f;
    private Rigidbody2D adheringSuzyRb;
    private float changeDirectionTime = 1.5f;
    private bool isStopMoving = false;
    [SerializeField] private Vector2 moveDirection;
    
    // Animation
    private Animator adheringSuzyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        // Find Start Position
        adheringSuzyRb = GetComponent<Rigidbody2D>();
        adheringSuzyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStopMoving)
        {
            Move();
        }
    }

    void Move()
    {
        // Open Eye When Move
        adheringSuzyAnimator.Play("Open Eye");
        adheringSuzyRb.velocity = moveDirection * moveSpeed;
    }

    IEnumerator ChangeDirection()
    {
        isStopMoving = true;
        moveDirection *= -1;
        yield return new WaitForSeconds(changeDirectionTime);

        isStopMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When Game Object collide Ground. Wait a few second and rotate game object
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Close Eye Not Move
            adheringSuzyAnimator.Play("Close Eye");
            StartCoroutine(ChangeDirection());
        }
    }
}
