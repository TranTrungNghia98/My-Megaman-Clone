using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnmies : MonoBehaviour
{
    private bool inRangeSpawn = false;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject spawnPosition;
    [SerializeField] private float starTime;
    [SerializeField] private float spawnRate;
    // Start is called before the first frame update
    void Awake()
    {
        InvokeRepeating("SpawnEnemy", starTime, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        if(inRangeSpawn)
        {
            Instantiate(enemyPrefab, spawnPosition.transform.position, enemyPrefab.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRangeSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inRangeSpawn = false;
        }
    }
}
