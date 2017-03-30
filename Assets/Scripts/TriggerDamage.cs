using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour {

	public float damage = 0;
	public bool destroyAfterHit = false;

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		if (destroyAfterHit) {
			Destroy(gameObject, 0);
		}
	}
}
