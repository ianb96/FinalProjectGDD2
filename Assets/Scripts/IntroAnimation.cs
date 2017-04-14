using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimation : MonoBehaviour {


	public TextMesh text;
	public GameObject groundSword;
	public GameObject mustGetSwordWall;
	Player player;
	GameObject playerSword;
	GameObject playerArms; 

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		playerSword = player.transform.FindChild("Sword").gameObject;
	}

	void Start()
	{
		playerSword.SetActive(false);
		// TODO enable temporary arms
		groundSword.SetActive(true);
		text.gameObject.SetActive(false);
		mustGetSwordWall.SetActive(true);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		text.gameObject.SetActive(true);
		// Fade in instead
	}
	void OnDisable()
	{
		// just in case
		playerSword.SetActive(true);
	}
	void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetButtonDown("Attack"))
		{
			// do an animation and wait for it
			playerSword.SetActive(true);
			groundSword.SetActive(false);
			text.gameObject.SetActive(false);
			mustGetSwordWall.SetActive(false);
		}
	}
}
