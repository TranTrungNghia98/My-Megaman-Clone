using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Enemy
{
    [SerializeField] GameObject[] blasterBulletPrefabs;
    [SerializeField] GameObject bulletPosition;
    private int bulletNumber = 0;
    private bool isDefendMode = false;
    private float shootRate = 0.5f;

    private Animator blasterAnimator;
    // Start is called before the first frame update
    void Start()
    {
        blasterAnimator = GetComponent<Animator>();
        // Choose Random Mode from beginning
        ChooseRandomMode();
    }

    void ChooseRandomMode()
    {
        float randomNumber = Random.Range(0.0f, 1.0f);
        if (randomNumber <= 0.5f)
        {
            StartCoroutine(ChangeToDefendMode());
        }

        else
        {
           StartCoroutine(ChangeToAttackMode());
        }
    }

    IEnumerator ChangeToDefendMode()
    {
        // Go to defend mode. Can't get damage from player
        isDefendMode = true;
        // Play Defend Animation
        blasterAnimator.Play("Defend");
        // Defend a few second then go to attack mode
        float defendTime = 4.0f;
        yield return new WaitForSeconds(defendTime);
        StartCoroutine(ChangeToAttackMode());
    }

    IEnumerator ChangeToAttackMode()
    {
        
        // Go out defend mode. Can get damage from player
        isDefendMode = false;
        // Play Attack Animation 
        blasterAnimator.Play("Attack");
        float animationLength = blasterAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Wait to end animation and shoot Bullet
        yield return new WaitForSeconds(animationLength);

        // Shoot Bullet
        while (bulletNumber < blasterBulletPrefabs.Length)
        {
            // Wait A few time then shoot
            yield return new WaitForSeconds(shootRate);
            // Shoot type of bullet depend bullet Number
            Instantiate(blasterBulletPrefabs[bulletNumber], bulletPosition.transform.position, blasterBulletPrefabs[bulletNumber].transform.rotation);
            // Change bullet number to spawn diffirent bullet in the next shoot
            bulletNumber++;
        }

        // Reset bullet Number
        bulletNumber = 0;
        // Wait a few seconds then go to defend mode
        float changeModeTime = 1.0f;
        yield return new WaitForSeconds(changeModeTime);
        StartCoroutine(ChangeToDefendMode());

    }

    protected override void GetDamage()
    {
        if (!isDefendMode)
        {
            float damage = 10;
            health -= damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player Bullet"))
        {
            Destroy(collision.gameObject);
            GetDamage();
            CheckHealth();
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            playerStatsScript = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStatsScript.GetDamage(damage);
        }
    }

}
