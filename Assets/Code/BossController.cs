using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public DungeonCharacter dungeonChar;
    public EnemyController controller;
    public Animator animator;

    // variables for attack
    public LayerMask playerLayer;
    public Transform attackPoint1;
    public Transform attackPoint2;
    public Transform attackPoint3;
    public float attackRadius1 = 1f;
    public float attackRadius2 = 2f;
    public float attackRadius3 = 3f;

    public int attackHitPoints = 1;

    public float jumpCD = 3f;
    public float jumpCDCount = 0;
    public bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.checkGameOver())
        {
            return;
        }
        PlayerController player = FindObjectOfType<PlayerController>();
        float dist = player.gameObject.transform.position.x - gameObject.transform.position.x;

        if (Time.time > jumpCDCount && dungeonChar.isAlive 
            && !dungeonChar.isAttacked && !dungeonChar.isAttacking && dungeonChar.isGrounded)
        {
            animator.SetTrigger("jumpAttack");
            animator.SetBool("isJumping", true);
            dungeonChar.isAttacking = true;
            jumpCDCount = Time.time + jumpCD;
            isJumping = true;
            controller.jump = true;
        } else if (Mathf.Abs(dist) < controller.minGap && dungeonChar.isAlive
            && !dungeonChar.isAttacked && !dungeonChar.isAttacking && dungeonChar.isGrounded)
        {
            animator.SetTrigger("attack");
            dungeonChar.isAttacking = true;
        }

        if (dungeonChar.isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
    }

    public void Attack1()
    {
        Collider2D[] player = Physics2D.OverlapCapsuleAll(
            attackPoint1.position, new Vector2(attackRadius1 * 2, attackRadius1*2), CapsuleDirection2D.Horizontal, 0, playerLayer);
        foreach (Collider2D p in player)
        {
            PlayerController pc = p.GetComponent<PlayerController>();
            if (pc)
            {
                pc.takeDamage(attackHitPoints);
            }
        }
    }

    public void Attack2()
    {
        Collider2D[] player = Physics2D.OverlapCapsuleAll(
            attackPoint2.position, new Vector2(attackRadius2 * 2, attackRadius2), CapsuleDirection2D.Horizontal, 0, playerLayer);
        foreach (Collider2D p in player)
        {
            PlayerController pc = p.GetComponent<PlayerController>();
            if (pc)
            {
                pc.takeDamage(attackHitPoints * 2);
            }
        }
    }

    public void Attack3()
    {
        Collider2D[] player = Physics2D.OverlapCapsuleAll(
            attackPoint3.position, new Vector2(attackRadius3 * 2, attackRadius3), CapsuleDirection2D.Horizontal, 45, playerLayer);
        foreach (Collider2D p in player)
        {
            PlayerController pc = p.GetComponent<PlayerController>();
            if (pc)
            {
                pc.takeDamage(attackHitPoints * 5);
            }
        }
    }

    public void OnLanding()
    {
        isJumping = false;
        if (!dungeonChar.isAttacking)
        {
            animator.SetBool("isJumping", false);
        }
    }

    public void jumpAttackDone()
    {
        dungeonChar.isAttacking = false;
        animator.SetBool("isJumping", false);
    }

    private void OnDrawGizmosSelected()
    {
        controller.drawGiz(new Transform[] { attackPoint1, attackPoint2, attackPoint3 }, 
            new float[] { attackRadius1, attackRadius2, attackRadius3 });
    }
}
