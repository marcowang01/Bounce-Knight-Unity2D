using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DungeonCharacter : MonoBehaviour
{
	// variables for movement
	public bool isFacingRight = true;
	[SerializeField] private float smoothingTime = .05f;

	// variables for jumping
	public float jumpForce = 400f;
	public LayerMask groundLayer;                          
	public Transform groundCheck;

	const float groundedRadius = .2f; 
	public bool isGrounded;            

	private Vector3 inputVelocity = Vector3.zero;

	// variables for attack/damage
	public bool isAttacked = false;
	public bool isAttacking = false;
	public bool isAlive = true;

	// varaibles for health bar
	public int maxHitPoints = 3;
	public int hitPoints = 3;
	public Sprite healthBarIcon;


	// events for movement
	public UnityEvent OnLandEvent;

	private Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		isAlive = true;
	}


	public void Move(float v, bool jump)
	{
		if ((isAttacking && isGrounded) || isAttacked || !isAlive)
        {
			v = 0;
        }
		Vector3 targetVelocity = new Vector2(v * 10f, 0);
		Vector3 tempVel = rb.velocity;
		tempVel.x = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref inputVelocity, smoothingTime).x;
		rb.velocity = tempVel;

		if (v > 0 && !isFacingRight)
		{
			Flip();
		}

		else if (v < 0 && isFacingRight)
		{
			Flip();
		}

		if (isGrounded && jump)
        {
			rb.AddForce(new Vector2(0f, jumpForce));
			isGrounded = false;
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = isGrounded;
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);

		foreach (Collider2D c in colliders)
        {
			if (c)
			{
				isGrounded = true;
				if (!wasGrounded && rb.velocity.y < 0)
                {
					OnLandEvent.Invoke();
					isAttacking = false;
				}
				break;
			}
        }
	}

	private void OnDrawGizmosSelected()
	{
		if (groundCheck != null)
		{
			Gizmos.DrawWireSphere(groundCheck.position, groundedRadius);
		}
	}


	public void Flip()
	{
		isFacingRight = !isFacingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void finishedTakingDamage()
	{
		isAttacked = false;
	}

	public void onAlive()
	{
		isAlive = true;
	}

	public void onAttackFinish()
	{
		isAttacking = false;
	}
}
