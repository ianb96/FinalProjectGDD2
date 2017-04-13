using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAI
{


    public Transform projectileSpawnPos;
    public Transform gunRoter;
    public GameObject projectilePrefab;

    public new void Start()
    {
        base.Start();
        anim.SetFloat("Speed", 1f / attackRate);
    }
    public override void Update()
    {
        base.Update();
        if (attackTimer > 0)
        {
            gunRoter.right = (transform.position - player.transform.position) * (facingRight ? 1 : -1);
        }
        float playerDist = (player.transform.position - transform.position).magnitude;
        if (playerDist <= attackRange)
        {
            anim.SetBool("Charging", true);
            // if (attackTimer <= 0)
            //     Attack();
        }
        else
        {
            anim.SetBool("Charging", false);
            attackTimer = attackRate;
        }
    }
    public void Attack()
    {
        anim.SetBool("Attacking", true);
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
        projectileGO.transform.right = player.transform.position - projectileSpawnPos.position;
        projectileGO.GetComponent<TriggerDamage>().damage = damage;
        attackTimer = attackRate;
    }
}
