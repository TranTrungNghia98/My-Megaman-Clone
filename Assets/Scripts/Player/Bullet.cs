using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D bulletRb;
    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        MoveForward();
        StartCoroutine(DestroyBullet());
    }

    void MoveForward()
    {
        bulletRb.AddForce(transform.right * speed, ForceMode2D.Impulse);
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

}
