// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class SwordRepulser : MonoBehaviour {
// 	public float length = 7.2f;
// 	Rigidbody2D rb;
// 	Rigidbody2D playerRb;
// 	/// <summary>
// 	/// Awake is called when the script instance is being loaded.
// 	/// </summary>
// 	void Awake()
// 	{
// 		rb = GetComponent<Rigidbody2D>();
// 		playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
// 	}
// 	/// <summary>
// 	/// Sent when an incoming collider makes contact with this object's
// 	/// collider (2D physics only).
// 	/// </summary>
// 	/// <param name="col">The Collision2D data associated with this collision.</param>
// 	void OnCollisionStay2D(Collision2D collision)
// 	{
// 		// Debug.Log("collision "+collision.collider.gameObject.layer);
// 		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Level"))
// 		{
// 			foreach (ContactPoint2D contact in collision.contacts)
// 			{
// 				if (contact.point.x < transform.position.x - length + 1)
// 				{
// 					ColliderDistance2D dist = rb.Distance(collision.collider);
// 					Debug.Log("in"+ dist.distance);
// 					if (dist.isOverlapped)
// 					{
// 						Debug.Log("overlapp");
// 						playerRb.AddForce(10*dist.normal*dist.distance, ForceMode2D.Impulse);
// 					}
// 				}
// 			}
// 		}
// 	}
// }
