using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : Damageable {
	Animator anim;
	void Awake()
	{
		anim = GetComponent<Animator>();
	}
	public override void Die()
	{
		if (anim)
			anim.SetBool("Dead",true);
		else
			DestroyGO();
	}
	public void DestroyGO()
	{
		Destroy(gameObject);
	}
}
