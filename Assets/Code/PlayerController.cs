using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public DungeonCharacter controller;
    public Animator animator;

    // varaibles for movement
    public float speed = 1f;
    float xDisplacement = 0f;
    bool isJumping = false;
    bool jump = false;

    // variables for attack
    public Transform attackPoint;
    public float attackRadius = 5f;
    public Transform jumpAttackPoint;
    public LayerMask enemyLayer;
   
    public float attackCD = 0.5f;
    private float attackCDCount = 0;
    public int jumpAttackHP = 5;

    // variables for damage
    public int maxHP = 3;
    public int hitPoints = 3;
    public Vector3 spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DungeonCharacter>();
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        xDisplacement = Input.GetAxis("Horizontal") * speed;
        animator.SetFloat("speed", xDisplacement * xDisplacement);

        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping && !controller.isAttacking)
            {
                isJumping = true;
                jump = true;
                animator.SetBool("isJumping", true);
            }
        }

        if (Input.GetButtonDown("Attack") && Time.time > attackCDCount && !controller.isAttacked)
        {
            animator.SetTrigger("attack");
            attackCDCount = Time.time + attackCD;
        }
    }


    private void FixedUpdate()
    {
        controller.Move(xDisplacement * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public void Attack()
    {
        int hp = 1;
        controller.isAttacking = true;
        Collider2D[] enemies;
        if (isJumping)
        {
            enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
            attackCDCount = Time.time;
            hp = jumpAttackHP;
        } else
        {
            Vector2 size = new Vector2(attackRadius * 2, attackRadius);
            enemies = Physics2D.OverlapCapsuleAll(
                attackPoint.position, size, CapsuleDirection2D.Horizontal, 0, enemyLayer);
        }
        foreach (Collider2D e in enemies)
        {
            EnemyController ec = e.GetComponent<EnemyController>();
            if (ec)
            {
                ec.takeDamage(hp);
            }
        }
    }

    public void takeDamage()
    {
        if (hitPoints > 0)
        {
            hitPoints -= 1;
            controller.isAttacked = true;
            controller.isAttacking = false;
            animator.SetTrigger("damaged");
            Debug.Log("player hit");
        }
    }

    public void OnLanding()
    {
        isJumping = false;
        controller.isAttacking = false;
        animator.SetBool("isJumping", false);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        if (jumpAttackPoint != null)
        {
            Gizmos.DrawWireSphere(jumpAttackPoint.position, attackRadius);
        }
    }
}
