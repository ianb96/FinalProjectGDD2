using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGiant : Boss
{

    //[HeaderAttribute("Giant")]


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (aggro)
        {
            if (phase == 0)
            {
                anim.SetFloat("Speed", 1);
            }
            else if (phase == 1)
            {
                anim.SetFloat("Speed", 0);
            }
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        Debug.Log("hit! " + name + " for " + amount);
        if (curHealth / maxHealth < 0.75f)
        {
            phase = 1;
            //...
        }
    }
}
