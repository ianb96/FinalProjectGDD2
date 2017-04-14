using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{

    public AudioClip walkSound;
    public AudioClip swingSound;
    public AudioClip attackChargeUpSound;

    AudioSource src;
    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayAttackChargeUp()
    {
		// src.PlayOneShot(attackChargeUpSound);
    }
    public void PlayWalkSound()
    {

    }
    public void PlaySwingSound()
    {
        
    }
}
