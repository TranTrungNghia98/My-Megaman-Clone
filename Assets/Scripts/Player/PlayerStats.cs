using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerAnimator playerAnimatorScript;
    private PlayerMovement playerMovementScript;
    private SpriteRenderer spriteRenderer;

    private float playerHealth = 100.0f;
    public bool isHurting { get; private set; }
    private float hurtingTime;
    private bool isInvincible;
    private float invincibleTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimatorScript = GetComponent<PlayerAnimator>();
        playerMovementScript = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeath();
    }

    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    public void GetDamage(float damage)
    {
        // After get damaged, player won't get damaged on a few seconds
        if (!isInvincible)
        {
            // Check logic
            isInvincible = true;
            playerHealth -= damage;
            isHurting = true;
            // Animation
            // PLay hurt animation, find length hurt animtion. Wait after hurt animation can change to other animation
            playerAnimatorScript.PlayAnimation("Hurt");
            hurtingTime = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(TurnIsHurtingToFalse());

            // Player when fall down when get damaged
            playerMovementScript.ResetGravity();
            StartCoroutine(DeactiveInvincibleMode());
        }
        
    }

    IEnumerator TurnIsHurtingToFalse()
    {
        yield return new WaitForSeconds(hurtingTime);
        isHurting = false;
    }

    IEnumerator DeactiveInvincibleMode()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    void CheckHeath()
    {
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Bullet"))
        {
            // Get Bullet Damage
            float enemyBulletDamage = 10.0f;
            GetDamage(enemyBulletDamage);
        }
    }
}
