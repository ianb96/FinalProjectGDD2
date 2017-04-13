using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Damageable
{
    public Screen playerScreen;
    [HeaderAttribute("Movement")]
    public bool canMove = true;
    public float walkSpeed = 3;
    public float runSpeed = 5;
    public float walkDur = 4;
    float targetSpeed = 0;
    public float maxSpeed = 10;
    float walkTimer = 0;
    public bool grounded = false;
    public int numJumps = 2;
    [ContextMenuItemAttribute("RecalculateJumpArc", "RecalculateJumpArc")]
    public float jumpHeight = 5;
    public float holdJumpHeight = 5;
    public float jumpDist = 4;
    int curJumps;
    float jumpDur = 0;
    float jumpSpeed = 0;
    float holdJumpSpeed = 0;
    float jumpBaseHeight = 0;
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
    public Screen deadScreen;

    [HeaderAttribute("Other")]
    public SpriteRenderer psprite;
    public CameraMove cam;
    public Transform swordAnim;
    public Transform swordPhys;
    public ParticleSystem doubleJumpEffect;
    public SpriteRenderer lightHalo;
    public Image vignette;
    public List<Light> lights;
    // int levelLayer;
    Rigidbody2D rb;
    Animator anim;
    PlayerSound playerSound;
    LevelManager lm;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSound = GetComponent<PlayerSound>();
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    new void Start()
    {
        base.Start();
        RecalculateJumpArc();
        SetSwordDamage();
        // levelLayer = 1 << LayerMask.NameToLayer("Level");
        Respawn();
        if (showGUI)
            playerScreen.Show();
    }

    /// Uses jumpDist, jumpHeight, and maxSpeed to determine jumpDur, jumpSpeed, and grav
    public void RecalculateJumpArc()
    {
        jumpDur = 0.5f * jumpDist / walkSpeed;
        jumpSpeed = 2 * jumpHeight / jumpDur;
        grav = -jumpSpeed / jumpDur;
        holdJumpSpeed = 2 * holdJumpHeight / jumpDur;
    }

    /// Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            TakeDamage(1000, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            numJumps = numJumps==200?2:200;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            movingSwordDamage = 200;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Time.timeScale = Time.timeScale < 1 ? 1 : 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            maxHealth = -2;
        }

        if (Time.timeScale == 0)
            return;
        isInWater = Physics2D.OverlapCircle(transform.position, 0.2f, 1 << LayerMask.NameToLayer("Water"));

        // gravity
        rb.AddForce(grav * Vector2.up, ForceMode2D.Force);
        anim.SetBool("Grounded", grounded);

        if (transform.position.y < -100)
        {
            transform.position = new Vector3(transform.position.x, 40, 0);
            TakeDamage(1, gameObject);
        }

        if (!canAttack)
            return;

        if (Input.GetButtonDown("Attack"))
        {
            anim.SetBool("Charging", true);
            attacking = true;
            anim.SetBool("Attacking", true);
            anim.ResetTrigger("Swing");
            StartCoroutine(CopySwordRotation(1.5f));
            attackCharge = 0;

            // TODO: in-air attack
            // if running increase speed / damage ?
            anim.SetFloat("SwingModifier", walkTimer<0?2:1);
            //if (grounded || isInWater)
            // else if (!downhit.collider)
            // {
            //     anim.SetTrigger("DownwardStrike");
            // }
        }
        if (Input.GetButtonUp("Attack"))
        {
            anim.SetBool("Charging", false);
            anim.SetTrigger("Swing");
        }
        if (!Input.GetButton("Attack"))
        {
            anim.SetBool("Charging", false);
        }
    }

    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        if (Time.timeScale == 0)
            return;
        if (canMove)
        {
            Move();
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
        rb.velocity = Vector2.Lerp(rb.velocity, desiredSpeed, 30 * Time.deltaTime);
        //rb.AddForce(desiredSpeed, ForceMode2D.Force);
        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }
        // sprite facing
        if (hor < 0)
        {
            psprite.flipX = true;
            //transform.localScale = new Vector3(-1,1,1);
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
        float speed = Mathf.Abs(desiredSpeed.x);// rb.velocity.x
        if (speed < 0.1f)
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
        if (grounded && rb.velocity.y <= 0) // TODO: use wasGrounded to fix?
        {
            anim.SetBool("Falling", false);
            anim.SetBool("Jumping", false);
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

        bool canJump = (curJumps < numJumps - (grounded ? 1 : 0) || isInWater);
        // jumping
        if (Input.GetButtonDown("Jump") && canJump)
        {
            StartJump();
        }
        if (Input.GetButton("Jump"))
        {
            if (transform.position.y - jumpBaseHeight < holdJumpHeight )
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + holdJumpSpeed * Time.deltaTime);
            }
        }
    }
    void StartJump()
    {
        if (!isInWater)
            curJumps++;
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed * (isInWater ? 0.4f : 1));
        jumpBaseHeight = transform.position.y;
        anim.SetBool("Jumping", true);
        anim.SetBool("Falling", false);
        if (!grounded)
            doubleJumpEffect.Play();
    }
    /// Help anim sword and phys sword align when switching
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
        UpdateHaloA();
        anim.SetTrigger("Hit");
        walkTimer = walkDur;
        invincible = true;
        TakeControl();
        Invoke("GiveControl", 0.4f);
        rb.AddForce(knockbackVel, ForceMode2D.Impulse);
        Invoke("SetNotInvincible", hitInvincibilityDur);
    }
    public override void FullHeal()
    {
        base.FullHeal();
        UpdateHaloA();
    }
    public void UpdateHaloA()
    {
        float ratio = curHealth / maxHealth;
        lightHalo.color = new Color(1,1,1, ratio * 0.1f);
        lights.ForEach((e)=>e.color = new Color(1,1,1, ratio * 0.5f));
        vignette.color = new Color(0,0,0,1 - ratio * 0.75f + 0.1f);
    }

    void SetNotInvincible()
    {
        invincible = false;
    }

    public override void Die()
    {
        rb.velocity = Vector3.zero;
        anim.SetBool("Dead", true);
        deadScreen.Show();
    }
    /// move back to last checkpoint with full health
    public void Respawn()
    {
        FullHeal();
        UpdateHaloA();
        // full heal effect?
        anim.SetBool("Dead", false);
        transform.position = lm.GetCheckpoint().position;
        grounded = false;
        // animation?
    }
    public void ActivatedCheckpoint()
    {
        FullHeal();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        OnCollisionStay2D(other);
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
                cam.lastGroundHeight = transform.position.y;
                if (contact.collider.CompareTag("Platform"))
                {
                    transform.Translate(contact.collider.GetComponent<MovingPlatform>().deltaMovement);
                    //cam.lastGroundHeight = contact.collider.transform.position.y;
                }
                // if (grounded == false) {
                //     if (cam.lastGroundHeight+20 > transform.position.y)
                //     {
                //         // TODO: fall damage?
                //     }
                // }
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
