using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNextLevel : MonoBehaviour
{

    public bool activated = true;
    public float delay = 5f;
    public float delayTimer = 0;
    LevelManager lm;


    void Awake()
    {
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        // activated = false;
        delayTimer = delay;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (activated && delayTimer <= 0)
        {
            if (other.CompareTag("Player"))
            {
                lm.NextLevel();
                delayTimer = delay;
            }
        }
    }
}
