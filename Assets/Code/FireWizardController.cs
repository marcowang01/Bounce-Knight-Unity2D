using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWizardController : MonoBehaviour
{

    public DungeonCharacter dungeonChar;
    public EnemyController controller;
    public Animator animator;

    // variables for attack
    public float attackCD = 2f;
    public float CDCount = 0;
    public LayerMask playerLayer;

    public Transform attackPoint;
    public float attackRadius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
    }

    // Fire Wizard will move toward player while firing on regular intervals
    private void Update()
    {
        if (Time.time > CDCount && !dungeonChar.isAttacked && dungeonChar.isAlive)
        {
            animator.SetTrigger("attack");
            dungeonChar.isAttacking = true;
            CDCount = Time.time + attackCD;
        }
    }

    public void Attack()
    {
        Collider2D[] player = Physics2D.OverlapCapsuleAll(
            attackPoint.position, new Vector2(attackRadius * 2, attackRadius), CapsuleDirection2D.Horizontal, 0, playerLayer);
        foreach (Collider2D p in player)
        {
            PlayerController pc = p.GetComponent<PlayerController>();
            if (pc)
            {
                pc.takeDamage();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        controller.drawGiz(new Transform[] { attackPoint }, new float[] { attackRadius });
    }
}
