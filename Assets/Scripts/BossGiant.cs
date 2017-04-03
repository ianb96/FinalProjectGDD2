using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGiant : Damageable {

	[HeaderAttribute("Boss")]
	public Screen aggroScreen;
	public bool aggro = true;
	public int phase;
	public float damage;
	public List<TriggerDamage> hitboxes = new List<TriggerDamage>();

	Rigidbody2D rb;
	Animator anim;
    Player player;
	CameraMove cam;
	AudioSource src;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		// src = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		cam = Camera.main.GetComponent<CameraMove>();
    }

    new void Start() {
        base.Start();
		aggro = false;
		hitboxes.ForEach((hb)=>hb.damage = damage);
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

    public override void OnHit(float amount, GameObject attacker) {
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
		
		if (aggroScreen)
			aggroScreen.Hide();
    }
	public void CameraShake()
	{
		cam.StartCameraShake(0.3f, 0.3f);
	}
	public void Aggro()
	{
		aggro = true;
		if (aggroScreen)
			aggroScreen.Show();
	}
}
