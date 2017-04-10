using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI {

    public float followRange = 5;

	public override void Update()
	{
		base.Update();
		float playerDist = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (playerDist <= followRange)
        {
			if (playerDist <= attackRange)
			{
				anim.SetFloat("Speed", 0);
				// rb.velocity = new Vector2(0, rb.velocity.y);
				if (attackTimer <= 0)
					Attack();
			}
			else
			{
				anim.SetBool("Attacking", false);
            	anim.SetFloat("Speed", movementSpeed);
			}
            // rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
        }
		else
		{
			anim.SetFloat("Speed", 0);
		}
		
	}
	public void Attack()
    {
        // set anim to attack
        // a melee attack collider is enabled in the animation?
        anim.SetBool("Attacking", true);
        attackTimer = attackRate;
    }

}
