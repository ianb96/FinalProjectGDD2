using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxHealth = 5;
	public float curHealth = 5;
	public float moveSpeed = 2;

	public Rigidbody2D rb;

	void Start () {
		
	}
	
	void Update () {
		float vx = Input.GetAxis("Horizontal");
		float vy = Input.GetAxis("Vertical");

		rb.velocity = new Vector2(vx, vy);
	}

	void Attack() {

	}

	void Respawn() {
		curHealth = maxHealth;
	}
}
