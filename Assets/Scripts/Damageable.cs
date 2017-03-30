using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public float maxHealth = 10;
    public float curHealth;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        curHealth = maxHealth;
    }
	
    public void TakeDamage(float amount) {
        curHealth-=amount;
        OnHit(amount);
        if (curHealth<=0) {
            Die();
        }
    }
    public virtual void Die() {
        // nothing
    }
    public virtual void OnHit(float amount) {
        // nothing
    }
}
