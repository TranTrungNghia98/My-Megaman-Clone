using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDoor : MonoBehaviour
{
    private float topBound = 80;
    private float bottomBound;
    private float moveSpeed = 10.0f;
    private bool isOpened = false;
    private bool isClosed = false;
    [SerializeField] GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        bottomBound = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpened)
        {
            OpenDoor();
        }

        else if (isClosed)
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
       door.SetActive(false);
    }

    private void CloseDoor()
    {
        door.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Open Door"))
            {
                OpenDoor();
            }

            else if (gameObject.CompareTag("Close Door"))
            {
                CloseDoor();
            }
        }
    }
}