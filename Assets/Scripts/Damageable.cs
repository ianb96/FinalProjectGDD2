using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    [HeaderAttribute("Health")]
    [SerializeField]
    protected float maxHealth = 10;// negative for perma-invinciblity
    protected float curHealth;
    protected bool invincible = false;
    [SerializeField]
    protected bool showGUI = false;
    public Slider healthSlider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public void Start()
    {
        curHealth = maxHealth;
        if (showGUI)
        {
            healthSlider.value = 1;
        }
    }

    /// Call this to attack this game object
    /// reduces current health by amount, and handles UI
    public void TakeDamage(float amount, GameObject attacker)
    {
        if (invincible || maxHealth < 0)
            return;
        curHealth -= amount;
        OnHit(amount, attacker);
        if (curHealth <= 0)
        {
            curHealth = 0;
            Die();
        }
        if (showGUI)
        {
            healthSlider.value = curHealth / maxHealth;
        }
    }
    public virtual void Die()
    {
        // nothing
    }
    public virtual void OnHit(float amount, GameObject attacker)
    {
        // nothing
    }
}
