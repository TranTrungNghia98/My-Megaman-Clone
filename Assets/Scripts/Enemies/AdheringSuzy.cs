using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdheringSuzy : Enemy
{
    private float moveSpeed = 10.0f;
    private Rigidbody2D adheringSuzyRb;
    private float rotateTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        // Find Start Position
        adheringSuzyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
    }

    void MoveForward()
    {
        adheringSuzyRb.velocity = transform.right * moveSpeed;
    }

    IEnumerator ChangeRotation()
    {
        yield return new WaitForSeconds(rotateTime);

        float rotationY = transform.rotation.y;

        if (rotationY == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When Game Object collide Ground. Wait a few second and rotate game object
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Colide with " + collision.gameObject.name);
            StartCoroutine(ChangeRotation());
        }
    }
}
