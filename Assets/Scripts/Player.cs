using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Damageable
{

    public bool canMove = true;

    bool inAttack = false;
    Rigidbody rb;
    Animator anim;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
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
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        //rb.
    }
    
    // start the attack sequence
    IEnumerator Attack()
    {
        yield return null;
    }
}
