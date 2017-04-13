using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBird : Boss
{

    //[HeaderAttribute("Bird")]
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    void Update()
    {
        if (aggro)
        {
            float playerDist = transform.position.x - player.transform.position.x;
            // anim.SetFloat("Dist", playerDist);
            // set position after bombs dropped?
        }
    }
    public void DropBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation);
        bomb.GetComponent<TriggerDamage>().damage = damage;
    }
    public void FinishDropping()
    {
        // move to a position to the right of the player?
        // or just automatically go to a charge animation?
        // or can animation do this with dist?
        // or just translate to a certain point here?
    }
    public override void OnHit(float amount, GameObject attacker)
    {
        //Debug.Log("hit! " + name + " for " + amount);
        if (phase != 2 && curHealth / maxHealth < 0.5f)
        {
            phase = 2;
            anim.SetTrigger("Mad");
            // play sound
            // anim.SetFloat("MultSpeed", 1.5f);
            // will this mess up end position?
            // use a different attack animation entirely?
        }
    }
}
