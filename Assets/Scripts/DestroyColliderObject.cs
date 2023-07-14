using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyColliderObject : MonoBehaviour
{
    // Destroy other game object when go to dead zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
