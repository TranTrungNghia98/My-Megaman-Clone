using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    private PlayerStats playerStatsScript;
    private Rigidbody2D dropItemRb;
    [SerializeField] int healthPoint;
    // Start is called before the first frame update
    void Start()
    {
        dropItemRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerStatsScript = collision.gameObject.GetComponent<PlayerStats>();
            playerStatsScript.IncreaseHealth(healthPoint);
            Destroy(gameObject);
        }
    }

}
