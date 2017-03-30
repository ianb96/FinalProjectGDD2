using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Damageable {

    public override void OnHit(float amount) {
        Debug.Log("hit! "+name+" for "+amount);
    }
    public override void Die() {
        //death anim
        Debug.Log("killed "+name);
    }
	
}
