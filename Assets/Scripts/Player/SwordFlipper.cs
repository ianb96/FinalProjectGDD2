using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFlipper : MonoBehaviour
{

    public Player player;
    public Animator anim;
    float turningTimer = 0;

    void Start()
    {

    }

    void Update()
    {
        //if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name=="New State")
        if (turningTimer <= 0)
        {
            if (player.grounded)
            {
                if (transform.position.x > player.transform.position.x)
                {
                    anim.SetTrigger("FlipSword");
                    turningTimer = 2;
                }
            }
        }
        else
        {
            turningTimer -= Time.deltaTime;
        }
    }
}
