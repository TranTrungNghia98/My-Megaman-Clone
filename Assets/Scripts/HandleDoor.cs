using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDoor : MonoBehaviour
{
    private bool isOpened = false;
    private bool isClosed = false;
    [SerializeField] GameObject door;

    //Sound Effect
    private AudioSource audioSource;
    [SerializeField] private AudioClip doorSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    // ABSTRACTION
    private void OpenDoor()
    {
        door.SetActive(false);
        audioSource.PlayOneShot(doorSound);
    }

    private void CloseDoor()
    {
        door.SetActive(true);
        audioSource.PlayOneShot(doorSound);
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
