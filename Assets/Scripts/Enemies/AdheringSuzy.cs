using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdheringSuzy : Enemy
{
    private float moveSpeed = 10.0f;
    private Rigidbody2D adheringSuzyRb;
    private float rotateTime = 1.5f;
    private bool isStopMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        // Find Start Position
        adheringSuzyRb = GetComponent<Rigidbody2D>();
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
        adheringSuzyRb.velocity = transform.right * moveSpeed;
    }

    IEnumerator ChangeDirection()
    {
        isStopMoving = true;
        moveSpeed *= -1;
        yield return new WaitForSeconds(rotateTime);

        isStopMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When Game Object collide Ground. Wait a few second and rotate game object
        if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(ChangeDirection());
        }
    }
}
