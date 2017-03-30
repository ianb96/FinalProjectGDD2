using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Damageable {

    
    public override void Die() {
        //death anim
        Debug.Log("hit! "+name);
    }
	
}
