﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnake : Boss
{

    [HeaderAttribute("Snake")]
	public float moveSpeed = 4; 
	// only move at beginning to get player on top, then just have a moving background and wind effects
	bool isPlayerOn = false;

    // Update is called once per frame
    void Update()
    {
        if (aggro)
        {
            if (phase == 0)
            {

            }
            anim.SetFloat("Speed", 1);
            // anim.SetInteger("Turning", 0);
        } else {
			// just fly around, try to get player on
		}
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        Debug.Log("hit! " + name + " for " + amount);
        
    }
    public void BulbDestroyed()
    {
        phase++;
    }
}