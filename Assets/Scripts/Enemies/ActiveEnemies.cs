using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemies : MonoBehaviour
{
    [SerializeField] GameObject enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemies.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemies.SetActive(false);
        }
    }
}
