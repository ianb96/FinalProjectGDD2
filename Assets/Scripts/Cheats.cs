using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour {

	bool cheatsEnabled;
	int codeEntryProgress = 0;
	Player player;
	LevelManager lm;

	void Awake () {
// #if UNITY_EDITOR
		// cheatsEnabled = true;
// #else
		cheatsEnabled = false;
// #endif
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();		
		lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	
	void Update () {
		if (!cheatsEnabled)
		{
			// check for a code of ```
			if (Input.GetKeyDown("`"))
			{
				codeEntryProgress++;
				if (codeEntryProgress>=3)
				{
					cheatsEnabled = true;
					Debug.Log("Cheats Enabled!");
				}
			}
			return;		
		}
        // cheats
		if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            // return to main menu
			lm.LoadLevel(0);
        }
		if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            lm.StartPrevLevel();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
           lm.NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            lm.MoveToNextCheckpoint();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            lm.ReloadLevel();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            player.TakeDamage(1000, gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
			// infinite jumps
            player.numJumps = 1000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
			// insta kill anything
            player.movingSwordDamage = 1000;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Time.timeScale = Time.timeScale < 1 ? 1 : 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            player.PermaInvincibility();
        }
	}
}
