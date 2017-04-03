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
    public Transform projectileSpawnPos;
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
    }
    /// returns the initial speed of an arc going to point b with gravity
    public Vector3 CalculateArc(Vector2 relTargetPos)
    {
        // float dur = 1f;
        // float height = 2f;
        // float yvel = 2 * height / dur;
        // float grav = -yvel / dur;
        // float xvel = 0.5f * relTargetPos.x / dur;

        // just shoot straight instead
        float xvel = relTargetPos.x;
        float yvel = relTargetPos.y;
        float grav = 0;
        return new Vector3(xvel, yvel, grav);
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (attackTimer>0)
        {
            attackTimer-=Time.deltaTime;
        }
        float playerDist = Mathf.Abs(player.transform.position.x - transform.position.x); 
        if (playerDist <= attackRange)
        {
            anim.SetFloat("Speed", 0);
            if (attackTimer<=0)
                Attack();
        }
        else if (!isRangedType && playerDist <= followRange)
        {
            // root anim to move ?
            anim.SetFloat("Speed", movementSpeed);
        }
        if (player.transform.position.x >= transform.position.x)
        {
            if (facingRight)
            {
                fliper.localScale = new Vector3(-1,1,1);
                facingRight = false;
            }
        } else {
            if (!facingRight)
            {
                fliper.localScale = new Vector3(1,1,1);
                facingRight = true;
            }
        }
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        Debug.Log("hit! " + name + " for " + amount+" by "+attacker.name);
        Vector2 knockBackDir = new Vector2((transform.position.x > player.transform.position.x ? 1 : -1) * knockbackForce.x, knockbackForce.y);
        rb.AddForce(knockBackDir, ForceMode2D.Impulse);
        gameObject.layer = LayerMask.NameToLayer("DmgProof");
        Invoke("DefLayer", 1);
    }
    public void Attack()
    {
        // set anim to attack
        if (isRangedType)
        {
            Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
            projectilePrefab.transform.LookAt(player.transform);
            Vector3 arcParams = CalculateArc(transform.position - player.transform.position);
            // projectilePrefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(arcParams.x, arcParams.y), ForceMode2D.Impulse);
            projectilePrefab.GetComponent<Rigidbody2D>().velocity = new Vector2(arcParams.x, arcParams.y);
            // projectilePrefab.GetComponent<Rigidbody2D>().gravityScale = arcParams.z / Physics2D.gravity.y;
            projectilePrefab.GetComponent<TriggerDamage>().damage = damage;
        }
        attackTimer = attackRate;
    }
    public override void Die()
    {
        //death anim
        DestroyGO(); // anim should call this
        Debug.Log("killed " + name);
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
