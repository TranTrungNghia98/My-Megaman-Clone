using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimator playerAnimatorScript;
    private PlayerMovement playerMovementScript;
    private PlayerSoundEffect playerSoundEffectScript;

    [SerializeField] private Transform bulletPosition;
    [SerializeField] private GameObject bulletPrefab;
    private float shootTime = 0;
    private float shootRate = 0.3f;
    public bool isPlayingShootAnimation { get; private set; }
    private float animationLength;
    private void Start()
    {
        playerAnimatorScript = GetComponent<PlayerAnimator>();
        playerMovementScript = GetComponent<PlayerMovement>();
        playerSoundEffectScript = GetComponent<PlayerSoundEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player press Z and current time pass time need to wait to need shoot. Player can shoot
        // Time next shoot = shootTime + shootRate
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= shootTime + shootRate)
        {
            // Play Shoot Animation
            ChooseShootAnimation();
            // Sound Effect
            playerSoundEffectScript.PlaySoundEffect("Shoot");
            // Spawn Bullet
            SpawnBullet();
        }
    }

    // ABSTRACTION
    void SpawnBullet()
    {
        shootTime = Time.time;
        Instantiate(bulletPrefab, bulletPosition.position, transform.rotation);
    }

    void ChooseShootAnimation()
    {
        isPlayingShootAnimation = true;
        // Idle or run shoot animation
        if (playerMovementScript.isGrounded())
        {
            if (playerMovementScript.horizontalInput == 0)
            {
                playerAnimatorScript.PlayAnimation("Idle Shoot");
            }
            
            else
            {
                playerAnimatorScript.PlayAnimation("Run Shoot");
            }
        }
        // Climb Shoot Animation
        else if (playerMovementScript.isClimbingStair)
        {
            playerAnimatorScript.PlayAnimation("Climb Shoot");
        }
        // Jump Shoot Animation
        else
        {
            playerAnimatorScript.PlayAnimation("Jump Shoot");
        }
        // Wait animation end and change shooting animation To False. So player can perform other animation
        animationLength = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(ChangeIsShootingAnimationToFalse());
    }

    IEnumerator ChangeIsShootingAnimationToFalse()
    {
        yield return new WaitForSeconds(animationLength);
        isPlayingShootAnimation = false;
    }
}
