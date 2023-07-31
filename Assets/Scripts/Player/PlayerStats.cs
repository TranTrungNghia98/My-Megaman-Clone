using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    private PlayerAnimator playerAnimatorScript;
    private PlayerMovement playerMovementScript;
    private PlayerSoundEffect playerSoundEffectScript;

    private float playerHealth = 100.0f;
    // ENCAPSULATION
    public bool isHurting { get; private set; }
    private float hurtingTime;
    private bool isInvincible;
    private float invincibleTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimatorScript = GetComponent<PlayerAnimator>();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerSoundEffectScript = GetComponent<PlayerSoundEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHeath();
    }

    // ENCAPSULATION
    public float GetPlayerHealth()
    {
        return playerHealth;
    }

    // ABSTRACTION
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
            // Sound Effect
            playerSoundEffectScript.PlaySoundEffect("Hurt");
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
            // Destroy player
            Destroy(gameObject);
            // Explosion Effect
            Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);
        }

        else if (playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    public void IncreaseHealth(int healthPoint)
    {
        playerHealth += healthPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Bullet"))
        {
            // Get Bullet Damage
            float enemyBulletDamage = 10.0f;
            GetDamage(enemyBulletDamage);
        }

        else if (collision.gameObject.CompareTag("DeadZone"))
        {
            float damage = 100;
            GetDamage(damage);
        }
    }
}
