using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ScreenManager : MonoBehaviour
{

    public bool loadInitialScreen = true;
    public Screen initialScreen;
    public Screen pauseScreen;
    public AudioMixer am;
    bool paused = false;
    // List<Screen> curScreens;

    void Start()
    {
        if (loadInitialScreen && initialScreen)
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
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         // Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
    public void SetMusicVolume(float value)
    {
        am.SetFloat("VolumeMusic", value);
    }
    public void SetSfxVolume(float value)
    {
        am.SetFloat("VolumeSfx", value);
    }
}
