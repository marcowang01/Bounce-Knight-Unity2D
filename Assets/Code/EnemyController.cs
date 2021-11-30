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

    public int maxHitPoints = 3;
    public int hitPoints = 3;

    // variables for movement
    public float speed = 10;
    private float xDisplacement;
    private float prevXDisplacement;
    public float minGap = 5;


    void Start()
    {
        controller = GetComponent<DungeonCharacter>();
        animator = GetComponent<Animator>();

        xDisplacement = speed;
        prevXDisplacement = speed;
    }

    private void Update()
    {
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

        float dist = player.gameObject.transform.position.x - gameObject.transform.position.x;
        if (Mathf.Abs(dist) < minGap)
        {
            xDisplacement = 0;
            if (player.gameObject.transform.position.x < gameObject.transform.position.x && controller.isFacingRight)
            {
                controller.Flip();
            }
            else if (player.gameObject.transform.position.x > gameObject.transform.position.x && !controller.isFacingRight)
            {
                controller.Flip();
            }
        }
        else
        {
            xDisplacement = prevXDisplacement;
        }

        animator.SetFloat("speed", Mathf.Abs(xDisplacement));
        if ((gameObject.transform.position.x < -8 && xDisplacement < 0)
            || (gameObject.transform.position.x > 8 && xDisplacement > 0))
        {
            xDisplacement = -xDisplacement;
        }
    }

    private void FixedUpdate()
    {
        if (controller.isAttacking || hitPoints == 0)
        {
            controller.Move(0, false);
        }
        else
        {
            controller.Move(xDisplacement, false);
        }
    }


    public void takeDamage(int hp)
    {
        if (controller.isAlive)
        {
            if (hitPoints > hp)
            {
                animator.SetTrigger("damaged");
                controller.isAttacked = true;
                controller.isAttacking = false;
                hitPoints -= hp;
                Debug.Log("Hit Enemy");
            }
            else
            {
                animator.SetTrigger("die");
                controller.isAlive = false;
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
