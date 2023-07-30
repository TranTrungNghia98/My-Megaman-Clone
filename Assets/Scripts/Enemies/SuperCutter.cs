using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCutter : Enemy
{
    private Rigidbody2D supperCutterRb;
    private Vector3 playerPosition;
    private float jumpForce = 5.0f;
    private float moveSpeed = 10.0f;
    private float selfDestructTime = 3.0f;

    // Sound Effect
    private AudioSource audioSource;
    [SerializeField] private AudioClip cuttingSound;

    // Start is called before the first frame update
    void Start()
    {
        supperCutterRb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Jump();
        StartCoroutine(SelfDestruct());
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void Update()
    {
        LookAtPlayer();
    }

    void Jump()
    {
        playerPosition = GameObject.Find("Player").transform.position;
        supperCutterRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        audioSource.PlayOneShot(cuttingSound);
    }

    void MoveToPlayer()
    {
        Vector3 moveDirection = (playerPosition - transform.position).normalized;
        supperCutterRb.velocity = new Vector2(moveDirection.x * moveSpeed, supperCutterRb.velocity.y);
    }

    void LookAtPlayer()
    {
        if(playerPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3 (1, 1, 1);
        }

        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);
    }

}
