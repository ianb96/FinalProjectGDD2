using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{

    public bool canMove = true;
    public float walkSpeed = 3;
    public float runSpeed = 5;
    public float walkDur = 4;
    float walkTimer = 0;
    bool grounded = false;
    public int numJumps = 2;
    int curJumps;
    public float jumpHeight = 5;
    public float jumpDist = 4;
    float jumpDur = 2;
    float jumpSpeed = 2;
    float grav = 10;

    bool attacking = false;
    bool inAttackSwing = false;
    int attackCharge = 0;
    int maxAttackCharges = 3;
    public float[] attackChargeDamages = {8, 10, 12, 15};
    
    public TriggerDamage swordHB;
    public SpriteRenderer psprite;
    Rigidbody2D rb;
    Animator anim;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    void Start()
    {
        RecalculateJumpArc();
    }

    /// Uses jumpDist, jumpHeight, and maxSpeed to determine jumpDur, jumpSpeed, and grav
    public void RecalculateJumpArc()
    {
        jumpDur = 0.5f * jumpDist / walkSpeed;
        jumpSpeed = 2 * jumpHeight / jumpDur;
        grav = -jumpSpeed / jumpDur;
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (canMove)
        {
            Move();
        }
        if (Input.GetButtonDown("Attack"))
        {
            attacking = true;
            anim.SetBool("Attacking", true);
            anim.SetBool("Swing", false);
            attackCharge = 0;
            // StartCoroutine(Attack());
        }
        if (Input.GetButtonUp("Attack"))
        {
            if (!inAttackSwing)
            {
                attacking = false;
                anim.SetBool("Attacking", false);
            } else {
                anim.SetBool("Swing", true);
            }
        }
    }

    /// Get input and move the player with rigidbody
    void Move()
    {
        // left / right movement
        float hor = Input.GetAxis("Horizontal");
        Vector2 desiredSpeed = new Vector2(0, rb.velocity.y);
        float optimalSpeed = 0;
        if (walkTimer<=0) {
            optimalSpeed = runSpeed;
        } else {
            optimalSpeed = walkSpeed;
        }
        if (Mathf.Abs(hor) > 0.05f)
        {
            if (walkTimer>0) {
                walkTimer -= Time.deltaTime;
            }
            if (desiredSpeed.x <= optimalSpeed)
            {
                desiredSpeed.x += hor * optimalSpeed * (attacking ? 0.2f:1f);
            }   
        } else {
            if (!Input.GetButton("Horizontal")) {
                // allow quick turns
                walkTimer = walkDur;
            }
        }
        rb.velocity = Vector2.Lerp(rb.velocity, desiredSpeed, 40 * Time.deltaTime);
        if (rb.velocity.x > optimalSpeed)
        {
            rb.velocity = new Vector2(optimalSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -optimalSpeed)
        {
            rb.velocity = new Vector2(-optimalSpeed, rb.velocity.y);
        }
        // sprite facing
        if (hor < 0)
        {
            psprite.flipX = true;
        }
        else if (hor > 0 || rb.velocity.x > 0.01)
        {
            psprite.flipX = false;
        }
        else if (rb.velocity.x < 0) // its like this to fix edge cases
        {
            psprite.flipX = true;
        }
        if (inAttackSwing)
        {
            psprite.flipX = false;
        }
        float speed = Mathf.Abs(rb.velocity.x);
        anim.SetFloat("Speed", speed > 0.05 ? speed : 0);

        // falling 
        if (grounded)
        {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
            curJumps = 0;
            rb.AddForce(-0.05f * Vector2.up, ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(grav * Vector2.up, ForceMode2D.Force);
            if (rb.velocity.y < 0)
            {
                anim.SetBool("Falling", true);
            }
        }

        // jumping
        if (curJumps < numJumps && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            anim.SetBool("Jumping", true);
            curJumps++;
        }
    }

    // start the attack sequence
    // IEnumerator Attack()
    // {
    //     // while
    //     yield return null;

    // }

    /// Anim will call this to indicate the point of no return for the attack swing
    public void AttackSwingStart()
    {
        inAttackSwing = true;
        if (attackCharge < maxAttackCharges)
        {
            anim.SetBool("MoreAttackCharges", true);
        } 
        else 
        {
            anim.SetBool("MoreAttackCharges", false);
        }
        swordHB.damage = attackChargeDamages[attackCharge];
    }
    /// Anim will call this to increase the current attack charge level
    public void SetAttackCharge(int num)
    {
        attackCharge = num;
        if (attackCharge < maxAttackCharges)
        {
            anim.SetBool("MoreAttackCharges", true);
        } 
        else 
        {
            anim.SetBool("MoreAttackCharges", false);
        }
        swordHB.damage = attackChargeDamages[attackCharge];
    }
    /// Anim will call this to indicate the attack swing is over
    public void AttackSwingEnd() {
        inAttackSwing = false;
        attacking = false;
        anim.SetBool("Attacking", false);
        walkTimer = walkDur;
    }

    public override void OnHit(float amount) {
        anim.SetTrigger("Hit");
        walkTimer = walkDur;
    }

    public override void Die() {
        // respawn
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionStay2D(Collision2D other)
    {
        //grounded = false;
        foreach (ContactPoint2D contact in other.contacts)
        {
            float dot = Vector3.Dot(contact.normal, Vector3.up);
            // Debug.Log(dot);
            if (dot > 0.8f)
            {
                // hit the ground
                grounded = true;
            }
            else if (dot > -0.2f && dot < 0.2f)
            {
                // hit a wall
                walkTimer = walkDur;
            }
        }
    }

    /// <summary>
    /// Sent when a collider on another object stops touching this
    /// object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionExit2D(Collision2D other)
    {
        grounded = false;
    }
}
