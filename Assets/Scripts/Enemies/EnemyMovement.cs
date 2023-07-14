using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject attackPositionPrefab;

    // Movement
    private Rigidbody2D enemyRb;
    private float speed = 10.0f;

    // Attack Movement
    private Vector3 attackRotationCenter;
    private float attackRadius = 4.0f;
    private float attackAngle = 0.0f;
    private float attackAngleSpeed = 2.0f;
    private float attackPosX, attackPosY = 0;
    private float attackRange = 5.0f;
    private bool inAttackMode = false;
    private bool isMoveLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inAttackMode)
        {
            AttackByElipse();
        }

        else
        {
            MoveForward();
        }

    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance <= attackRange)
        {
            inAttackMode = true;
        }
    }

    private void MoveForward()
    {
        enemyRb.velocity = transform.right * speed;
    }

    private void AttackByElipse()
    {
        // Receive move position to move
        Vector3 movePosition = CreateElipsePath();
        // Create prefab position to enemy move to it
        if (!GameObject.FindGameObjectWithTag("AttackPosition"))
        {
            Instantiate(attackPositionPrefab, movePosition, attackPositionPrefab.transform.rotation);
        }
        // Find direction to move
        // Find AttackPosition Game Object Position and move to it
        Vector3 moveDirection = GameObject.FindGameObjectWithTag("AttackPosition").transform.position - transform.position;
        enemyRb.velocity = moveDirection * speed;

    }

    // Create Elipse path to make enemy move by elipse
    private Vector3 CreateElipsePath()
    {
        Vector3 result;
        // Create rotation center above player, so enemy can attack player by elipse
        attackRotationCenter = player.transform.position + new Vector3(0, 2, 0);
        // Find Attack Position
        attackPosX = attackRotationCenter.x + Mathf.Cos(attackAngle) * attackRadius;
        attackPosY = attackRotationCenter.y + Mathf.Sin(attackAngle) * attackRadius / 2;
        result = new Vector3(attackPosX, attackPosY);

        return result;
    }

    // Change attack angle to change attack position. To crease Elipse Path
    private void ChangeAngleAttack()
    {
        if (isMoveLeft)
        {
            attackAngle -= Time.deltaTime * attackAngleSpeed;
        }
        
        else
        {
            attackAngle += Time.deltaTime * attackAngleSpeed;
        }

        if (attackAngle < - Mathf.PI)
        {
            isMoveLeft = false;
        }

        else if(attackAngle > 0) 
        {
            isMoveLeft = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AttackPosition"))
        {
            Destroy(collision.gameObject);
            ChangeAngleAttack();
        }
    }

}
