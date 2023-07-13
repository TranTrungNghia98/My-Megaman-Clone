using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject bulletPrefab;
    private float shootTime = 0;
    private float shootRate = 0.3f;

    // Update is called once per frame
    void Update()
    {
        // If player press A and current time pass time need to wait to need shoot. Player can shoot
        // Time next shoot = shootTime + shootRate
        if (Input.GetKeyDown(KeyCode.A) && Time.time >= shootTime + shootRate)
        {
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        shootTime = Time.time;
        Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
    }
}
