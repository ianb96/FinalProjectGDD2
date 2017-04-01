using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{

    public float maxHealth = 10;// negative for perma-invinciblity
    public float curHealth;
    public bool invincible = false;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public void Start()
    {
        curHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (invincible || maxHealth < 0)
            return;
        curHealth -= amount;
        OnHit(amount);
        if (curHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        // nothing
    }
    public virtual void OnHit(float amount)
    {
        // nothing
    }
}
