using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{
    public Screen playerScreen;
    [HeaderAttribute("Movement")]
    public bool canMove = true;
    public float walkSpeed = 3;
    public float runSpeed = 5;
    public float walkDur = 4;
    float targetSpeed = 0;
    float walkTimer = 0;
    public bool grounded = false;
    public int numJumps = 2;
    [ContextMenuItemAttribute("RecalculateJumpArc", "RecalculateJumpArc")]
    public float jumpHeight = 5;
    public float jumpDist = 4;
    int curJumps;
    float jumpDur = 2;
    float jumpSpeed = 2;
    float grav = -10;
    bool isInWater = false;

    [HeaderAttribute("Combat")]
    public Vector2 dodgeVel = new Vector2(-2, 4);
    public float dodgeDelay = 1;
    float dodgeTimer = 0;
    public Vector2 knockbackVel = new Vector2(-2f, 0.5f);

    public bool canAttack = true;
    bool attacking = false;
    bool inAttackSwing = false;
    int attackCharge = 0;
    public int maxAttackCharges = 3;
    public float[] attackChargeDamages = { 2, 10, 12, 15 };
    public float movingSwordDamage = 6;
    public float hitInvincibilityDur = 0.2f;
    public Vector2 knockbackForce = new Vector2(3, 2);
    public TriggerDamage[] swordHbs;

    [HeaderAttribute("Other")]
    public SpriteRenderer psprite;
    public CameraMove cam;
    public Transform swordAnim;
    public Transform swordPhys;
    public ParticleSystem doubleJumpEffect;
    int levelLayer;
    Rigidbody2D rb;
    Animator anim;
    PlayerSound playerSound;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSound>();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    new void Start()
    {
        base.Start();
        RecalculateJumpArc();
        SetSwordDamage();
        levelLayer = 1 << LayerMask.NameToLayer("Level");
        if (showGUI)
            playerScreen.Show();
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
        isInWater = Physics2D.OverlapCircle(transform.position, 0.2f, 1 << LayerMask.NameToLayer("Water"));

        // gravity
        if (grounded)
        {
            rb.AddForce(-0.05f * Vector2.up, ForceMode2D.Force);
        }
        else
        {
            rb.AddForce(grav * Vector2.up, ForceMode2D.Force);
        }
        anim.SetBool("Grounded", grounded);

        if (transform.position.y < -50)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        if (!canAttack)
            return;

        // don't swing sword if not enough room
        // RaycastHit2D uhit = Physics2D.Raycast(transform.position, Vector3.up, 4, levelLayer);
        // if (uhit.collider)
        // {
        //     return;
        // }

        if (Input.GetButtonDown("Attack"))
        {
            anim.SetBool("Charging", true);
            attacking = true;
            anim.SetBool("Attacking", true);
            anim.ResetTrigger("Swing");
            StartCoroutine(CopySwordRotation(1.5f));
            attackCharge = 0;

            //if (grounded || isInWater)
            {
                // if running increase speed / damage ?
                // walkTimer = walkDur;
            }
            // TODO: in-air attack
            // else if (!downhit.collider)
            // {
            //     anim.SetTrigger("DownwardStrike");
            // }
        }
        if (Input.GetButtonUp("Attack"))
        {
            anim.SetBool("Charging", false);
            anim.SetTrigger("Swing");
            //walkTimer = walkDur;
            // if (!inAttackSwing)
            // {
            //     attacking = false;
            //     anim.SetBool("Attacking", false);
            // }
            // else
            {
                // anim.SetBool("Swing", true);
                //rb.velocity = Vector3.zero;
            }
        }
        if (!Input.GetButton("Attack"))
        {
            anim.SetBool("Charging", false);
        }
    }

    /// Get input and move the player with rigidbody
    void Move()
    {
        // left / right movement
        float hor = Input.GetAxis("Horizontal");
        Vector2 desiredSpeed = new Vector2(0, rb.velocity.y);

        targetSpeed = Mathf.Lerp(targetSpeed, walkTimer <= 0 ? runSpeed : walkSpeed, 2 * Time.deltaTime);
        if (Mathf.Abs(hor) > 0.05f)
        {
            if (walkTimer > 0)
            {
                walkTimer -= Time.deltaTime;
            }
            if (desiredSpeed.x <= targetSpeed)
            {
                float attackModifier = ((hor < 0 && attacking) || anim.GetBool("Swing") ? 0.4f : 1f);
                desiredSpeed.x += hor * targetSpeed * attackModifier;
            }
        }
        else
        {
            if (!Input.GetButton("Horizontal") && walkTimer <= 0)
            {
                // allow quick turns
                walkTimer = walkDur;
            }
        }
        rb.velocity = desiredSpeed;
        //rb.AddForce(desiredSpeed, ForceMode2D.Force);
        // = Vector2.Lerp(rb.velocity, desiredSpeed, 30 * Time.deltaTime);
        // if (rb.velocity.x > targetSpeed)
        // {
        //     rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
        // }
        // else if (rb.velocity.x < -targetSpeed)
        // {
        //     rb.velocity = new Vector2(-targetSpeed, rb.velocity.y);
        // }
        // sprite facing
        if (hor < 0)
        {
            psprite.flipX = true;
        }
        else if (hor > 0 || rb.velocity.x > 0.01f)
        {
            psprite.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            // its like this to fix edge cases
            psprite.flipX = true;
        }
        if (inAttackSwing)
        {
            psprite.flipX = false;
        }
        float speed = Mathf.Abs(rb.velocity.x);
        if (speed < 0.01f)
            speed = 0;
        if (hor < 0 && attacking)
        {
            walkTimer = walkDur;
            speed *= -1;
        }
        anim.SetFloat("Speed", speed);
        if (!attacking && attackCharge == 0)
        {
            if (rb.velocity.sqrMagnitude > 1)
                SetSwordDamage(movingSwordDamage);
            else
                SetSwordDamage();
        }


        // falling 
        if (grounded && rb.velocity.y <= 0)
        {
            anim.SetBool("Falling", false);
            curJumps = 0;
        }
        else
        {
            if (rb.velocity.y < -1f)
            {
                anim.SetBool("Falling", true);
            }
        }

        // dodge
        if (dodgeTimer <= 0)
        {
            if (grounded && Input.GetButtonDown("Dodge"))// && rb.velocity.x<0)
            {
                rb.velocity = dodgeVel;
                anim.SetBool("Dodging", true);
                invincible = true;
                canMove = false;
                canAttack = false;
                Invoke("GiveControl", 1f); // backup give control
                psprite.flipX = false;
            }
        }
        else
        {
            dodgeTimer -= Time.deltaTime;
        }

        // jumping
        if (Input.GetButtonDown("Jump") && (curJumps < numJumps - (grounded ? 1 : 0) || isInWater))
        {
            if (!isInWater)
                curJumps++;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * (isInWater ? 0.4f : 1));
            anim.SetTrigger("Jumping");
            anim.SetBool("Falling", false);
            if (!grounded)
                doubleJumpEffect.Play();
        }
    }

    public IEnumerator CopySwordRotation(float dur)
    {
        float targetRotation = swordPhys.localEulerAngles.z;
        if (targetRotation > 180)
            targetRotation -= 360;
        float progress = 0;
        swordAnim.localEulerAngles = new Vector3(0, 0, targetRotation);
        yield return null;
        while (attacking && progress < 1)
        {
            progress += Time.deltaTime / dur;
            float nAngle = Mathf.Lerp(targetRotation, 0, progress);
            swordAnim.localEulerAngles = new Vector3(0, 0, nAngle);
            yield return null;
        }
        swordAnim.localEulerAngles = new Vector3(0, 0, 0);
    }

    /// Anim will call this to indicate the point of no return for the attack swing
    public void AttackSwingStart()
    {
        inAttackSwing = true;
    }
    /// Anim will call this to increase the current attack charge level
    public void SetAttackCharge(int num)
    {
        if (num >= attackChargeDamages.Length)
            Debug.LogError("atack charge too high!");
        attackCharge = num;
        playerSound.PlayAttackChargeUp();
        if (attackCharge < maxAttackCharges)
        {
            anim.SetBool("MoreAttackCharges", true);
        }
        else
        {
            anim.SetBool("MoreAttackCharges", false);
        }
        SetSwordDamage();
    }
    public void AttackSwingHit()
    {
        rb.velocity = Vector3.zero;
        cam.StartCameraShake(0.2f, 0.2f);
    }
    /// Anim will call this to indicate the attack swing is over
    public void AttackSwingEnd()
    {
        inAttackSwing = false;
        attacking = false;
        anim.SetBool("Attacking", false);
        anim.ResetTrigger("Swing");
        walkTimer = walkDur;
        attackCharge = 0;
        SetSwordDamage();
    }
    public void SetSwordDamage(float amount = -1)
    {
        foreach (TriggerDamage swordHB in swordHbs)
        {
            if (amount == -1)
                swordHB.damage = attackChargeDamages[attackCharge];
            else
                swordHB.damage = amount;
        }
    }

    public void FinishDodge()
    {
        SetNotInvincible();
        GiveControl();
        anim.SetBool("Dodging", false);
        rb.velocity = new Vector2(0, rb.velocity.y);
        walkTimer = walkDur;
        dodgeTimer = dodgeDelay;
    }

    public void TakeControl()
    {
        anim.SetFloat("Speed", 0);
        canMove = false;
        canAttack = false;
    }
    public void GiveControl()
    {
        canMove = true;
        canAttack = true;
    }

    public override void OnHit(float amount, GameObject attacker)
    {
        Debug.Log("hit by " + attacker.name + " for " + amount);
        anim.SetTrigger("Hit");
        walkTimer = walkDur;
        invincible = true;
        TakeControl();
        Invoke("GiveControl", 0.4f);
        rb.AddForce(knockbackVel, ForceMode2D.Impulse);
        Invoke("SetNotInvincible", hitInvincibilityDur);
    }

    void SetNotInvincible()
    {
        invincible = false;
    }

    public override void Die()
    {
        // respawn
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionStay2D(Collision2D other)
    {
        // if (other.collider.gameObject.layer == 1 << LayerMask.NameToLayer("Sword"))
        //     return;
        //grounded = false;
        foreach (ContactPoint2D contact in other.contacts)
        {
            float dot = Vector3.Dot(contact.normal, Vector3.up);
            // Debug.Log(dot);
            if (dot > 0.8f)
            {
                // hit the ground
                grounded = true;
                cam.lastGroundHeight = transform.position.y;
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
