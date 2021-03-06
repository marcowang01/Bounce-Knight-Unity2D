using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public DungeonCharacter controller;
    public Animator animator;

    // variables for damage
    public int pointsPerHp = 1;
    public int pointsPerDeath = 5;

    // variables for movement
    public float speed = 10;
    private float xDisplacement;
    private float prevXDisplacement;
    public float minGap = 5;

    public bool jump = false;


    void Start()
    {
        controller = GetComponent<DungeonCharacter>();
        animator = GetComponent<Animator>();

        xDisplacement = speed;
        prevXDisplacement = speed;
    }

    private void Update()
    {
        if (GameManager.checkGameOver())
        {
            return;
        }
        // follows player
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player.gameObject.transform.position.x < gameObject.transform.position.x && xDisplacement > 0)
        {
            xDisplacement = -xDisplacement;
        }
        else if (player.gameObject.transform.position.x > gameObject.transform.position.x && xDisplacement < 0)
        {
            xDisplacement = -xDisplacement;
        }
        if (xDisplacement != 0)
        {
            prevXDisplacement = xDisplacement;
        }

        // stops when enemy gets too close and flip facing dir accordingly
        float dist = player.gameObject.transform.position.x - gameObject.transform.position.x;
        if (Mathf.Abs(dist) < minGap)
        {
            xDisplacement = 0;
            if (player.gameObject.transform.position.x < gameObject.transform.position.x && controller.isFacingRight
                && !controller.isAttacking)
            {
                controller.Flip();
            }
            else if (player.gameObject.transform.position.x > gameObject.transform.position.x && !controller.isFacingRight
                && !controller.isAttacking)
            {
                controller.Flip();
            }
        }
        else
        {
            xDisplacement = prevXDisplacement;
        }

        // make sure enemy doesn't run off the map
        animator.SetFloat("speed", Mathf.Abs(xDisplacement));
        if ((gameObject.transform.position.x < -8 && xDisplacement < 0)
            || (gameObject.transform.position.x > 8 && xDisplacement > 0))
        {
            xDisplacement = -xDisplacement;
        }
    }

    private void FixedUpdate()
    {
        if ((controller.isAttacking && controller.isGrounded) || controller.hitPoints == 0)
        {
            controller.Move(0, jump);
        }
        else
        {
            controller.Move(xDisplacement, jump);
        }
        jump = false;
    }


    public void takeDamage(int hp)
    {
        controller.isAttacked = true;
        controller.isAttacking = false;
        if (controller.isAlive)
        {
            if (controller.hitPoints > hp)
            {
                animator.SetTrigger("damaged");
                controller.hitPoints -= hp;
            }
            else
            {
                controller.hitPoints = 0;
                animator.SetTrigger("die");
                controller.isAlive = false;
                GameManager.setGameWin();
            }
        }
    }

    public void die()
    {
        Destroy(gameObject);
    }

    public void drawGiz(Transform[] attackPoints, float[] radii)
    {
        for (int i = 0; i < attackPoints.Length; i++)
        {
            if (attackPoints[i])
                Gizmos.DrawWireSphere(attackPoints[i].position, radii[i]);
        
        }
    }
}
