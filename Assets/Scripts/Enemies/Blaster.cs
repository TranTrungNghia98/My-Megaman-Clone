using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Enemy
{
    [SerializeField] GameObject[] blasterBulletPrefabs;
    private int bulletNumber = 0;
    private bool isDefendMode = false;
    private float shootRate = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
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
        gameObject.GetComponent<SpriteRenderer>().color = new Color(113, 228, 0, 255);
        // Go to defend mode. Can't get damage from player
        isDefendMode = true;
        // Defend a few second then go to attack mode
        float defendTime = 4.0f;
        yield return new WaitForSeconds(defendTime);
        StartCoroutine(ChangeToAttackMode());
    }

    IEnumerator ChangeToAttackMode()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(228, 0, 88, 255);
        // Go out defend mode. Can get damage from player
        isDefendMode = false;

        // Shoot Bullet
        while (bulletNumber < blasterBulletPrefabs.Length)
        {
            // Wait A few time then shoot
            yield return new WaitForSeconds(shootRate);
            // Shoot type of bullet depend bullet Number
            Instantiate(blasterBulletPrefabs[bulletNumber], transform.position, blasterBulletPrefabs[bulletNumber].transform.rotation);
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
    }

}
