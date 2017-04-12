using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnakeBulb : Destructable {

	public BossSnake boss;

	
    public override void OnHit(float amount, GameObject attacker)
    {
        // do something?
    }
	public override void Die()
	{
		boss.TakeDamage(10, gameObject);
		if (anim)
			anim.SetBool("Dead",true);
		else
			DestroyGO();
	}
}
