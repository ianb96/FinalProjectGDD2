using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGiant : Boss
{

    //[HeaderAttribute("Giant")]
    public float attackRange = 6;
    public float Range = 6;
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (aggro)
        {
            float playerDist = transform.position.x - player.transform.position.x;
            anim.SetFloat("Dist", playerDist);
            anim.SetFloat("Speed", 0);
            // anim.SetInteger("AttackType", 0);
            if (playerDist < 0)
            {
                anim.SetTrigger("JumpBack");
                anim.SetBool("Attack", false);
            }
            else if (playerDist <= attackRange)
            {
                // anim.SetInteger("AttackType", phase+1);
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetFloat("Speed", 1);
                anim.SetBool("Attack", false);
            }
            // if (phase == 0)
            // if (phase == 1)
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        Debug.Log("hit! " + name + " for " + amount);
        if (phase != 2 && curHealth / maxHealth < 0.5f)
        {
            phase = 2;
            anim.SetTrigger("Mad");
            // play sound
            anim.SetFloat("MultSpeed", 1.5f);
        }
        // else if (curHealth / maxHealth < 0.75f)
        // {
        //     phase = 1;
        // }
    }
}
