using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{

    public bool canMove = true;
    public float maxSpeed = 4;
    public float accelRate = 0.5f;
    public int numJumps = 2;
    int curJumps;
    public float jumpHeight = 5;
    public float jumpDist = 4;
    float jumpDur = 2;
    float jumpSpeed = 2;
    float grav = 10;
    bool inAttack = false;
    public bool grounded = false;
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
        jumpDur = 0.5f * jumpDist / maxSpeed;
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
        if (!inAttack)
        {
            if (Input.GetButtonDown("Attack"))
            {
                StartCoroutine(Attack());
            }
        }
    }

    // Get input and move the player with rigidbody
    void Move()
    {
        // left / right
        float hor = Input.GetAxis("Horizontal");
        // float ver = Input.GetAxis("Vertical");
        if (Mathf.Abs(hor) > 0.05f) {
            if (rb.velocity.x <= maxSpeed && rb.velocity.x >= -maxSpeed)
            {
                rb.velocity += Vector2.right * hor * accelRate;
            }
            else if (rb.velocity.x > maxSpeed)
            {
                rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            }
            else if (rb.velocity.x < -maxSpeed)
            {
                rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
            }
        } else {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(0,rb.velocity.y), 100*accelRate*Time.deltaTime);
        }
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (rb.velocity.x < 0) {
            psprite.flipX = true;
        } else if (rb.velocity.x > 0) {
            psprite.flipX = false;
        }

        // falling 
        if (grounded) {
            anim.SetBool("Jumping", false);
            anim.SetBool("Falling", false);
            curJumps = 0;
            rb.AddForce(-0.05f*Vector2.up, ForceMode2D.Force);
        }else {
            rb.AddForce(grav*Vector2.up, ForceMode2D.Force);
            if (rb.velocity.y < 0) {
                anim.SetBool("Falling", true);
            }
        }
        // jumping
        if (curJumps < numJumps && Input.GetButtonDown("Jump"))
        {
            // rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            anim.SetBool("Jumping", true);
            curJumps++;
        }
    }
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionStay2D(Collision2D other)
    {
        float dot = Vector3.Dot(other.contacts[0].normal, Vector3.up);
        // Debug.Log(dot);
        grounded = false;
        if (dot > 0.75f)
        { 
            // hit the ground
            grounded = true;
        }
        else if (dot > 0.25f)
        {
            // hit a wall
            Debug.Log(other.relativeVelocity);
            rb.AddForce(2*other.relativeVelocity, ForceMode2D.Impulse);
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
    // start the attack sequence
    IEnumerator Attack()
    {
        yield return null;
    }
}
