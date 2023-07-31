using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriver : Enemy
{
    
    [SerializeField] private GameObject multipleBulletPrefabs;
    [SerializeField] private AudioClip shootSound;
    private AudioSource audioSource;
    private float shootCount = 0;
    private float shootRate = 1.5f;

    private GameObject player;
    private float attackRange = 10;
    private bool isStartingAttack = false;

    // Animation
    private Animator screwDriverAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        screwDriverAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(StartAttack());
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    // ABSTRACTION
    void CheckDistance()
    {
        Vector2 playerPosition = player.transform.position;
        float distance = Vector2.Distance(playerPosition, transform.position);

        if (distance <= attackRange)
        {
            isStartingAttack = true;
        }

    }

    // Move To Attack Poistion and Shoot 2 time and stop attack
    private IEnumerator StartAttack()
    {
        // Animation
        screwDriverAnimator.Play("Attack");

        // Change Position
        //transform.localPosition = transform.localPosition + transform.up;
        // Shoot 2 time and Stop attack. After shoot, wait a few time then shoot again
        while (shootCount < 2 && isStartingAttack)
        {
            //Sound Effect
            audioSource.PlayOneShot(shootSound);
            // Spawn Bullet
            Instantiate(multipleBulletPrefabs, transform.position, transform.rotation);
            // Increase shoot count to prevent infinite loop
            shootCount++;
            yield return new WaitForSeconds(shootRate);
        }

        // After shoot 2 time, Stop Attack
        StartCoroutine(StopAttack());


    }

    // Move to stat Position, wait a few second and attack again
    private IEnumerator StopAttack()
    {
        // Animation
        screwDriverAnimator.Play("Defend");
        // Reset Shoot Count, make game object can shoot again
        shootCount = 0;
        // Change position
        //transform.localPosition = transform.localPosition - transform.up;

        // After stop attack a few seconds, start attack again
        yield return new WaitForSeconds(shootRate);
        StartCoroutine(StartAttack());
    }

}
