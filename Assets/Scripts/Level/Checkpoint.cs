﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

	public Transform spawnPosition;
	public GameObject levelBoundary;
	public TriggerNextLevel nextLevelLoader;
	public TriggerNextLevel prevLevelLoader;
    LevelManager lm;
	int checkpointIndex;
	Animator anim;

    void Awake()
    {
		anim = GetComponent<Animator>();
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		if (!spawnPosition)
			spawnPosition = transform.GetChild(0);
    }

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			lm.ActivateCheckpoint(checkpointIndex);
			if (anim)
				anim.SetBool("Light", true);
		}
	}
	// public override void OnHit(float amount, GameObject attacker)
    // {
    //     if (attacker.CompareTag("Player"))
	// 	{
	// 		lm.ActivateCheckpoint(checkpointIndex);
	// 	}
    // }
	public void SetCheckpointIndex(int index)
	{
		checkpointIndex = index;
	}
}
