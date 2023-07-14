using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float playerHealth = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeath();
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public void GetDamage(float damage)
    {
        playerHealth -= damage;
    }

    void CheckHeath()
    {
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}
