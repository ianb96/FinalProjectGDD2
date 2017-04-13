using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNextLevel : MonoBehaviour
{

    public bool activated = true;
    public bool goToPreviousLevel = false;
    LevelManager lm;


    void Awake()
    {
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (activated)
        {
            if (other.CompareTag("Player"))
            {
                if (goToPreviousLevel)
                {
                    lm.PrevLevel();
                }
                else
                {
                    lm.NextLevel();
                }
            }
        }
    }
}
