using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewDriver : Enemy
{
    [SerializeField] private GameObject multipleBulletPrefabs;
    private float shootCount = 0;
    private float shootRate = 1.5f;
    private Vector2 startPosition;
    private Vector2 attackPosition;

    private GameObject player;
    private float attackRange = 10;
    private bool isStartingAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        attackPosition = startPosition + new Vector2(0, 2);
        player = GameObject.Find("Player");

        StartCoroutine(StartAttack());
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

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
        transform.position = attackPosition;
        // Shoot 2 time and Stop attack. After shoot, wait a few time then shoot again
        while (shootCount < 2 && isStartingAttack)
        {
            Instantiate(multipleBulletPrefabs, transform.position, multipleBulletPrefabs.transform.rotation);
            shootCount++;
            yield return new WaitForSeconds(shootRate);
        }

        // After shoot 2 time, Stop Attack
        StartCoroutine(StopAttack());


    }

    // Move to stat Position, wait a few second and attack again
    private IEnumerator StopAttack()
    {
        // Reset Shoot Count, make game object can shoot again
        shootCount = 0;

        transform.localPosition = startPosition;

        yield return new WaitForSeconds(shootRate);

        // After stop attack a few seconds, start attack again
        StartCoroutine(StartAttack());
    }

}