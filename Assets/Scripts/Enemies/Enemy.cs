using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected int score;
    [SerializeField] GameObject dropItemPrefab;
    protected PlayerStats playerStatsScript;
    protected GameManager gameManagerScript;

    protected virtual void GetDamage()
    {
        float damage = 10;
        health -= damage;
    }

    protected void DropItem()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        // 20% drop item
        if (randomNumber <= 0.2)
        {
            Instantiate(dropItemPrefab, transform.position, dropItemPrefab.transform.rotation);
        }
        
    }

    protected void CheckHealth()
    {
        // Destroy Enemy when health <= 0
        if (health <= 0)
        {
            Destroy(gameObject);
            // Add Score and 20% to Drop Item
            DropItem();
            gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gameManagerScript.AddScore(score);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
