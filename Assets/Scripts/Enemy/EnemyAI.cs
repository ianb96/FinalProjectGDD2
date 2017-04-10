using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Damageable
{
    [HeaderAttribute("EnemyAI")]
    public float movementSpeed = 2.5f;
    public float attackRate = 4f;
    protected float attackTimer = 0;
    public float damage = 1f;
    public float attackRange = 1;
    public Vector2 knockbackForce = new Vector2(6, 4);
    public List<TriggerDamage> hitboxes = new List<TriggerDamage>();
    public Transform fliper;
    protected bool facingRight = true;
    protected Rigidbody2D rb;
    protected Player player;
    protected Animator anim;
    Vector3 defScale;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public new void Start()
    {
        base.Start();
        attackTimer = Random.Range(0f, attackRate);
        hitboxes.ForEach((hb)=>hb.damage = damage);
        defScale = fliper.localScale;
    }
    
    public virtual void Update()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        // flip to face player
        if (player.transform.position.x >= transform.position.x)
        {
            if (facingRight)
            {
                fliper.localScale = new Vector3(-defScale.x, defScale.y, defScale.z);
                facingRight = false;
            }
        }
        else
        {
            if (!facingRight)
            {
                fliper.localScale = new Vector3(defScale.x, defScale.y, defScale.z);
                facingRight = true;
            }
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        // Debug.Log("hit! " + name + " for " + amount + " by " + attacker.name);
        if (curHealth > 0)
        {
            Vector2 knockBackDir = new Vector2((transform.position.x > player.transform.position.x ? 1 : -1) * knockbackForce.x, knockbackForce.y);
            rb.AddForce(knockBackDir, ForceMode2D.Impulse);
            gameObject.layer = LayerMask.NameToLayer("DmgProof");
            Invoke("DefLayer", 1);
        }
    }
    public override void Die()
    {
        //death anim
        anim.SetBool("Dead", true);
        // DestroyGO(); // anim should call this
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
