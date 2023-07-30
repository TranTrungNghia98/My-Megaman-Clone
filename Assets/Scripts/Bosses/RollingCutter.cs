using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCutter : MonoBehaviour
{
    private float damage = 15.0f;
    private Rigidbody2D rollingCutterRb;
    
    private float moveSpeed = 10.0f;
    // Get Player position to find Direction to move to the player
    private GameObject player;
    private Vector3 moveToPlayerDirection;
    // Get player Stats Script
    private PlayerStats playerStatsScript;
    // Set Time to move to player. If game object can hit player in this time. It will turn back to the boss
    private float moveToPlayerTime = 1.5f;
    // Get Cut Man Boss Position to find direction to move to the boss
    private GameObject cutManBoss;
    private CutMan cutManScript;
    private bool isMoveToBoss = false;

    // Sound Effect
    private AudioSource audioSource;
    [SerializeField] private AudioClip cuttingSound;
    // Start is called before the first frame update
    void Start()
    {
        // Get Rb
        rollingCutterRb = GetComponent<Rigidbody2D>();
        // Get player
        player = GameObject.Find("Player");
        playerStatsScript = player.GetComponent<PlayerStats>();
        // Get Boss
        cutManBoss = GameObject.Find("Cut Man");
        cutManScript = cutManBoss.GetComponent<CutMan>();
        // Get Audio Source
        audioSource = GetComponent<AudioSource>();
        // Play Sound Effect
        audioSource.PlayOneShot(cuttingSound);
        // Find Direcion to player
        moveToPlayerDirection = (player.transform.position - transform.position).normalized;
        // Move To Player and wait a few second to turn back the back
        StartCoroutine(WaitToTurnBack(moveToPlayerTime));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMoveToBoss)
        {
            MoveToPlayer();
        }

        else
        {
            MoveToBoss();
        }
    }

    // If game object can hit player in this time. It will turn back to the boss
    IEnumerator WaitToTurnBack(float timeToMoveBack)
    {
        yield return new WaitForSeconds(timeToMoveBack);
        isMoveToBoss = true;
    }

    // Move game object to player
    void MoveToPlayer()
    {
        rollingCutterRb.velocity = moveToPlayerDirection * moveSpeed;
    }

    // Move game object to the boss
    void MoveToBoss()
    {
        Vector3 moveToBossDirection = (cutManBoss.transform.position - transform.position).normalized;
        rollingCutterRb.velocity = moveToBossDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If colide player, it will wait a second and move to the boss
        if (collision.gameObject.CompareTag("Player"))
        {
            float waitTime = 0.3f;
            StartCoroutine(WaitToTurnBack(waitTime));
            playerStatsScript = GameObject.Find("Player").GetComponent<PlayerStats>();
            playerStatsScript.GetDamage(damage);
        }
        // If collide the boss and make boss Pick up Rolling Cutter
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            cutManScript.PickUpRollingCutter();
        }
    }
}
