using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Damageable
{

    [HeaderAttribute("Boss")]
	public string bossName = "Boss";
    public Screen aggroScreen;
	[SerializeField]
	[ContextMenuItemAttribute("Aggro", "Aggro")]
	[ContextMenuItemAttribute("DeAggro", "DeAggro")]
    protected bool aggro = false;
    public int phase;
    public float damage;
    public List<TriggerDamage> hitboxes = new List<TriggerDamage>();

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Player player;
    protected CameraMove cam;
    protected AudioSource src;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // src = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cam = Camera.main.GetComponent<CameraMove>();
    }

    new void Start()
    {
        base.Start();
        aggro = false;
        SetDamage(damage);
    }
	/// set our hitboxes to the damage amount
    public void SetDamage(float dam = -1)
    {
		if (dam != -1)
        	damage = dam;
        hitboxes.ForEach((hb) => hb.damage = damage);
    }
    public override void Die()
    {
        //death anim and stuff
        Debug.Log("killed " + name);
        anim.SetBool("Dead", true);
        if (aggroScreen)
            aggroScreen.Hide();
    }
    /// starts camera shake on the camera for .3s at .3mag
	public void CameraShake()
    {
        cam.StartCameraShake(0.3f, 0.3f);
    }
	/// enables agro on the boss
    public void Aggro()
    {
        aggro = true;
        if (aggroScreen)
		{
            aggroScreen.Show();
			aggroScreen.GetComponentInChildren<Text>().text = bossName;
		}
    }
	/// disables agro on the boss
    public void DeAggro()
    {
        aggro = false;
        if (aggroScreen)
		{
            aggroScreen.Hide();
		}
    }
}
