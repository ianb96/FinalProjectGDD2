using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{

    public float damage = 0;
    public bool destroyAfterHit = false;
	public bool hitStop = false;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Damageable>())
        {
            other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            if (destroyAfterHit)
            {
                Destroy(gameObject, 0);
            }
			if (hitStop)
			{
				Time.timeScale = 0.1f;
				Invoke("ResetTime", 0.025f);
			}
        }
    }
	void ResetTime()
	{
		Time.timeScale = 1f;
	}
}
