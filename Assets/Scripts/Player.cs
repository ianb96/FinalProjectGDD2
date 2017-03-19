using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxHealth;
	public float curHealth;

	public float moveSpeed;

	public CharacterController cc;

	void Start () {
		
	}
	
	void Update () {
		float vx = Input.GetAxis("Horizontal");
		float vz = Input.GetAxis("Vertical");

		
	}
}
