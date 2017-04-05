using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
{
    public float damage = 0;
    public bool destroyAfterHit = false;
	public bool hitStop = false;
    public float hitStopTime = 0.05f;

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (damage==0)
            return;
        Damageable dam = other.GetComponent<Damageable>();
        if (dam)
        {
            dam.TakeDamage(damage, transform.root.gameObject);
            // other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            if (destroyAfterHit)
            {
                StartCoroutine(FadeOut());
            }
			if (hitStop)
                StartCoroutine(HitStop());
        }
    }
    public IEnumerator HitStop()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(hitStopTime);
        Time.timeScale = 1f;
    }
    public IEnumerator FadeOut()
    {
        float progress = 0;
        float duration = 1;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        while (progress < 1)
        {
            progress += Time.deltaTime / duration;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f - progress);
            yield return null;
        }
        Destroy(gameObject);
    }
}
