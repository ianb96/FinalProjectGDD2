using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimCollision : MonoBehaviour {

	Animator anim;
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		anim = GetComponentInParent<Animator>();
	}
	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="collision">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Level"))
		{
			foreach (ContactPoint2D contact in collision.contacts)
			{
				if (contact.point.y > transform.position.y)
				{
					anim.SetBool("Attacking", false);
				}
			}
		}
	}
}
