using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioSource playerAudioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    // Start is called before the first frame update
    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ABSTRACTION
    public void PlaySoundEffect(string name)
    {
        switch(name) {
            case "Shoot":
                playerAudioSource.PlayOneShot(shootSound);
                break;
            case "Jump":
                playerAudioSource.PlayOneShot(jumpSound);
                break;
            case "Hurt":
                playerAudioSource.PlayOneShot(hurtSound);
                break;
            default:
                return;
        }
    }
}
