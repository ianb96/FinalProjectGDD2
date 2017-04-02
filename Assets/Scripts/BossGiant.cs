using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGiant : Damageable {


	public bool aggro = true;
	public int phase;
	public TriggerDamage[] hitboxes;

	Rigidbody2D rb;
	Animator anim;
    Player player;
	CameraMove cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		cam = Camera.main.GetComponent<CameraMove>();
    }

    new void Start() {
        base.Start();
		//aggro = false;
    }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (aggro)
		{
			if (phase==0)
			{
				anim.SetFloat("Speed", 1);
			}
			else if (phase == 1)
			{
				anim.SetFloat("Speed", 0);
			}
		}
	}


    public override void OnHit(float amount) {
        Debug.Log("hit! "+name+" for "+amount);
        if (curHealth/maxHealth < 0.75f)
		{
			phase = 1;
			//...
		}
    }
    public override void Die() {
        //death anim and stuff
        Debug.Log("killed "+name);
    }
	public void CameraShake()
	{
		cam.StartCameraShake(0.3f, 0.3f);
	}
}
