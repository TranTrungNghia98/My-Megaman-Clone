using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mambu : Enemy
{
    private Rigidbody2D mambuRb;
    private float moveSpeed = 7.0f;
    private bool isDefendMode;
    private float switchModeTime = 2.0f;
    private float selfDestructTime = 4.0f;
    [SerializeField] GameObject multipleBulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        mambuRb = GetComponent<Rigidbody2D>();
        // Choose Random Mode To Create interesting experience
        ChooseRandomMode();

        // Destroy after few seconds
        StartCoroutine(SelfDestruct());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDefendMode)
        {
            MoveForward();
        }
    }

    // Choose Random Mode
    private void ChooseRandomMode()
    {
        // Create a number to choose mode
        float randomNumber = Random.Range(0.0f, 1.0f);
        // Generate 50 % Go To Defend Mode or Go To Attack Mode
        if (randomNumber <= 0.5f)
        {
            StartCoroutine(GoToDefendMode());
        }

        else
        {
            StartCoroutine(GoToAttackMode());
        }
    }

    private void MoveForward()
    {
        mambuRb.velocity = transform.right * moveSpeed;
    }

    // Go To Defend Mode in few seconds then shoot bullets
    private IEnumerator GoToDefendMode()
    {
        // Turn defend mode = true. So gameobject can't get damage when in defend mode
        isDefendMode = true;
        ContinueMoving();
        // Change Color to easy recognize in defend Mode
        gameObject.GetComponent<SpriteRenderer>().color = new Color(113, 228, 0, 255);
        
        // Wait few second and change Mode
        yield return new WaitForSeconds(switchModeTime);
        // Shoot Bullets
        StartCoroutine(GoToAttackMode());
    }

    // Shoot MultiBullet at the same time then wait few seconds then go to defend mode
    private IEnumerator GoToAttackMode()
    {
        // Turn defend mode = false. So game object can get damage from player
        isDefendMode = false;

        // Turn to origin color
        gameObject.GetComponent<SpriteRenderer>().color = new Color(252, 116, 96, 255);

        // Stop Moving And Attack
        StopMoving();
        Instantiate(multipleBulletPrefab, transform.position, multipleBulletPrefab.transform.rotation);

        // Wait few second and change Mode
        yield return new WaitForSeconds(switchModeTime);
        // Go to Defend Mode
        StartCoroutine(GoToDefendMode());
    }

    private void StopMoving()
    {
        // Freezee X position so player can't move
        mambuRb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    private void ContinueMoving()
    {
        // Reset contraints so palyer can move
        mambuRb.constraints = RigidbodyConstraints2D.None;
        mambuRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructTime);
        Destroy(gameObject);

    }

    // Change Get Damage Method So game object can't get damage when in defend mode
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
