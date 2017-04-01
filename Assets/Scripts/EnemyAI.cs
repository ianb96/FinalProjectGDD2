using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Damageable {


    public Vector2 knockbackForce = new Vector2(6, 4);
    Rigidbody2D rb;
    Player player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    new void Start() {
        base.Start();
    }

    public override void OnHit(float amount) {
        Debug.Log("hit! "+name+" for "+amount);
        Vector2 knockBackDir = new Vector2( (transform.position.x>player.transform.position.x ? 1:-1) * knockbackForce.x, knockbackForce.y);
        rb.AddForce(knockBackDir, ForceMode2D.Impulse);
        gameObject.layer = LayerMask.NameToLayer("DmgProof");
        Invoke("DefLayer", 1);
    }
    public override void Die() {
        //death anim
        Debug.Log("killed "+name);
    }
    void DefLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Creature");
    }
	
}
