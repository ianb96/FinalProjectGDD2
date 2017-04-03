using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

	public Screen initialScreen;
	public Screen pauseScreen;
	bool paused = false;
	// List<Screen> curScreens;

	void Start () {
		DontDestroyOnLoad(gameObject);
		if (initialScreen)
			ShowScreen(initialScreen);
	}
	
	void Update () {
		if (Input.GetButtonDown("Pause"))
		{
			if (paused)
				ShowScreen(pauseScreen);
			else
				HideScreen(pauseScreen);
		}
	}

	public void ShowScreen(Screen newScreen)
	{
		newScreen.SMShow();
		newScreen.transform.SetAsLastSibling();
		// add to list?
	}
	public void HideScreen(Screen newScreen)
	{
		newScreen.Hide();
	}
}
