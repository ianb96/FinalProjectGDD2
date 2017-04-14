using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBird : Boss
{

    //[HeaderAttribute("Bird")]
    public float moveSpeed = 4;
    public float attackRate = 5;
    public GameObject mover;
    public GameObject bombPrefab;
    public Transform bombSpawnPoint;
    public Transform bombrunStartPoint;
    public Door openWhenDead;
    float attackTimer = 0;
    public new void Start()
    {
        base.Start();
        attackTimer = attackRate;
    }
    void Update()
    {
        if (aggro)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                float playerDist = transform.position.x - player.transform.position.x;
                int attackType = Random.Range(0f, 1f) > 0.67f ? 1 : 2;
                if (playerDist < 0 || playerDist > 8)
                    attackType = 1;

                anim.SetInteger("Attack", attackType);
                // 1 is fly around, 2 is peck
            }
        }
    }
    public void DropBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation);
        bomb.GetComponent<TriggerDamage>().damage = damage;
        bomb.GetComponent<Arrow>().projectileSpeed = 15;
        bomb.transform.localScale = bombSpawnPoint.localScale;
    }
    public void PrepareBombRun()
    {
        StartCoroutine(MoveTo(bombrunStartPoint.position));
    }
    public void FinishBombRun()
    {
        Vector3 choosenPos = new Vector3(player.transform.position.x + 5, bombrunStartPoint.position.y, 0);
        StartCoroutine(MoveTo(choosenPos));
    }
    IEnumerator MoveTo(Vector3 targetPos)
    {
        float distance = (targetPos - mover.transform.position).magnitude;
        float progress = 0;
        Vector3 startPointm = mover.transform.position;
        Vector3 targetm = new Vector3(targetPos.x, targetPos.y, 0);
        Vector3 startPointb = transform.position;
        Vector3 targetb = new Vector3(targetPos.x, transform.position.y, 0);
        while (progress < 1)
        {
            progress += moveSpeed / distance * Time.deltaTime;
            mover.transform.position = Vector3.Lerp(startPointm, targetm, progress);
            transform.position = Vector3.Lerp(startPointb, targetb, progress);
            yield return null;
        }
        mover.transform.position = targetm;
        transform.position = targetb;
        anim.SetTrigger("InPosition");
    }
    public void Landed()
    {
        anim.SetInteger("Attack", 0);
        attackTimer = attackRate;
    }
    public override void OnHit(float amount, GameObject attacker)
    {
        //Debug.Log("hit! " + name + " for " + amount);
        if (phase != 2 && curHealth / maxHealth < 0.5f)
        {
            phase = 2;
            anim.SetTrigger("Mad");
            attackRate *= 0.75f;
            // play sound
            // anim.SetFloat("MultSpeed", 1.5f);
            // will this mess up end position?
            // use a different attack animation entirely?
        }
        if (curHealth <= 0)
        {
            openWhenDead.Open();
        }
    }
}
