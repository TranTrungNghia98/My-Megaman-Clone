using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleEffect : MonoBehaviour
{
    // Animation
    private Animator effectAnimator;
    private float effectTime;
    // Sound Effect
    private AudioSource effectAudioSource;
    [SerializeField] private AudioClip deadSound;
    // Start is called before the first frame update
    void Start()
    {
        effectAnimator = GetComponent<Animator>();
        effectAudioSource = GetComponent<AudioSource>();

        // Sound Effect
        effectAudioSource.PlayOneShot(deadSound);

        // Animation
        effectAnimator.Play("Explosion");
        effectTime = effectAnimator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, effectTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
