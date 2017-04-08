using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{

    public Screen initialScreen;
    public Screen pauseScreen;
    bool paused = false;
    // List<Screen> curScreens;

    void Start()
    {
        if (initialScreen)
        {
            ShowScreen(initialScreen);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }
	/// show the screen
    public void ShowScreen(Screen newScreen)
    {
        newScreen.SMShow();
        newScreen.transform.SetAsLastSibling();
        // add to list?
    }
	/// hide the screen
    public void HideScreen(Screen newScreen)
    {
        newScreen.Hide();
    }
	public void TogglePause()
	{
		if (paused)
		{
			ShowScreen(pauseScreen);
			Time.timeScale = 0;
		}
		else
		{
			HideScreen(pauseScreen);
			Time.timeScale = 1;
		}
		paused = !paused;
	}
}
