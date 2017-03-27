using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public float maxHealth;
    public float curHealth;
	
    public void TakeDamage(float amount) {
        curHealth-=amount;
        if (curHealth<=0) {
            Die();
        }
    }
    public void Die() {
        // nothing
    }

}
