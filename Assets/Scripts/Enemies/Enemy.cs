using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damage;
    [SerializeField] GameObject dropItemPrefab;
    private PlayerStats playerStatsScript;

    private void Start()
    {
        playerStatsScript = GameObject.Find("Player").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        CheckHealth();
    }

    protected void GetDamage()
    {
        float damage = 10;
        health -= damage;
    }

    protected void DropItem()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        // 50% drop item
        if (randomNumber >= 0.5)
        {
            Instantiate(dropItemPrefab, transform.position, dropItemPrefab.transform.rotation);
        }
        
    }

    protected void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            // 50% drop item when be destroy
            GetDamage();
            DropItem();
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            playerStatsScript.GetDamage(damage);
        }
    }

}
