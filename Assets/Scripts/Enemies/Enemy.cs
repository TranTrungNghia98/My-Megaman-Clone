using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PlayerStats playerStatsScript;
    private GameManager gameManagerScript;

    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float enemyDamage;
    [SerializeField] protected int enemyScore;
    [SerializeField] GameObject dropItem;
    private GameObject player;
    private Rigidbody2D enemyRb;
    private float speed = 10.0f;
    private bool isFollowPlayer = false;
    private float xOffset = 10;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerStatsScript = player.GetComponent<PlayerStats>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        if (isFollowPlayer)
        {
            FollowPlayer();
        }

        else
        {
            MoveFoward();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyHealth();
        CheckOffset();
    }

    // Enemy will follow player when them near by player
    private void CheckOffset()
    {
        if (player.transform.position.x - transform.position.x <= xOffset)
        {
            isFollowPlayer = true;
        }
    }

    // Get Damage Method
    protected void GetDamage()
    {
        float damage = 10.0f;
        enemyHealth -= damage;
    }

    // Check Health Method
    protected void CheckEnemyHealth()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Movement Methods
    protected void FollowPlayer()
    {
        Vector3 followDirection = (player.transform.position - transform.position).normalized;

        enemyRb.velocity = followDirection * speed;
    }

    protected void MoveFoward()
    {
        enemyRb.velocity = transform.right * speed;
    }

    // Drop Item
    protected void DropItem()
    {
        float randomNumber = Random.Range(0, 1.0f);

        if (randomNumber >= 0.5f)
        {
            Instantiate(dropItem, transform.position, dropItem.transform.rotation);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            GetDamage();
            DropItem();
            gameManagerScript.AddScore(enemyScore);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            playerStatsScript.GetDamage(enemyDamage);
        }
    }
}
