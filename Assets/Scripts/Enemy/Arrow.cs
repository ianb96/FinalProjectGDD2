using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : TriggerDamage {
	public float projectileSpeed = 10;
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		rb.velocity = transform.right*projectileSpeed;
	}
	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.collider.CompareTag("Player")) {
			rb.velocity = Vector2.zero;
			GetComponent<TriggerDamage>().damage = 0;
		}
		StartCoroutine(FadeOut());
	}
}
