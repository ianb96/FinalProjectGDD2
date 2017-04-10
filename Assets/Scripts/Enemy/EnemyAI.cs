using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Damageable
{
    [HeaderAttribute("EnemyAI")]
    public bool isRangedType = false;
    public float movementSpeed = 2.5f;
    public float attackRate = 4f;
    float attackTimer = 0;
    public float damage = 1f;
    public float attackRange = 1;
    public float followRange = 5;
    public Vector2 knockbackForce = new Vector2(6, 4);
    public List<TriggerDamage> hitboxes = new List<TriggerDamage>();
    public Transform projectileSpawnPos;
    public Transform gunRoter;
    public GameObject projectilePrefab;
    public Transform fliper;
    bool facingRight = true;
    Rigidbody2D rb;
    Player player;
    Animator anim;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    new void Start()
    {
        base.Start();
        attackTimer = Random.Range(0f, attackRate);
        hitboxes.ForEach((hb)=>hb.damage = damage);
        if (isRangedType)
            anim.SetFloat("Speed", attackRate);
    }
    
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (attackTimer > 0)
        {
            if (isRangedType)
                gunRoter.right = transform.position - player.transform.position;
            attackTimer -= Time.deltaTime;
        }
        float playerDist = Mathf.Abs(player.transform.position.x - transform.position.x);
        if (playerDist <= attackRange)
        {
            if (!isRangedType)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                anim.SetFloat("Speed", 0);
            }
            if (attackTimer <= 0)
                Attack();
        }
        else if (!isRangedType && playerDist <= followRange)
        {
            anim.SetFloat("Speed", movementSpeed);
            rb.velocity = new Vector2(-movementSpeed, rb.velocity.y);
        }
        if (player.transform.position.x >= transform.position.x)
        {
            if (facingRight)
            {
                fliper.localScale = new Vector3(-1, 1, 1);
                facingRight = false;
            }
        }
        else
        {
            if (!facingRight)
            {
                fliper.localScale = new Vector3(1, 1, 1);
                facingRight = true;
            }
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        // Debug.Log("hit! " + name + " for " + amount + " by " + attacker.name);
        Vector2 knockBackDir = new Vector2((transform.position.x > player.transform.position.x ? 1 : -1) * knockbackForce.x, knockbackForce.y);
        rb.AddForce(knockBackDir, ForceMode2D.Impulse);
        gameObject.layer = LayerMask.NameToLayer("DmgProof");
        Invoke("DefLayer", 1);
    }
    public void Attack()
    {
        // set anim to attack
        anim.SetBool("Attacking", true);
        if (isRangedType)
        {
            GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
            projectileGO.transform.right = player.transform.position - projectileSpawnPos.position;
            projectileGO.GetComponent<TriggerDamage>().damage = damage;
        }
        else
        {
            // a melee attack collider is enabled in the animation
        }
        attackTimer = attackRate;
    }
    public override void Die()
    {
        //death anim
        DestroyGO(); // anim should call this
        // Debug.Log("killed " + name);
    }

    /// called by anim when death anim finishes
    public void DestroyGO()
    {
        Destroy(gameObject, 0);
    }
    /// Sets the layer to Creature
    void DefLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Creature");
    }
}
