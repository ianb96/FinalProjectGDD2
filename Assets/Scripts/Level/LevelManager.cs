using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// Manages the current scene
public class LevelManager : MonoBehaviour
{

    [ContextMenuItemAttribute("MoveToNextCheckpoint", "MoveToNextCheckpoint")]
    public bool RCToNextCP;
    public int loadSceneImmediately = -1;
    public Screen loadingScreen;
    public Screen endGameScreen;
    Slider loadingSlider;
    public float minLoadingTime = 1;
    int curSceneIndex = 0;
    int curCheckpoint = 0;
    Player player;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        loadingSlider = loadingScreen.GetComponentInChildren<Slider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        if (loadSceneImmediately > 0)
            LoadLevel(loadSceneImmediately);
    }
    public void NextLevel()
    {
        curCheckpoint = 0;
        LoadLevel(curSceneIndex + 1);
    }
    public void StartPrevLevel()
    {
        if (curSceneIndex < 1)
            return;
        curCheckpoint = 0;
        LoadLevel(curSceneIndex - 1);
    }
    public void PrevLevel()
    {
        if (curSceneIndex < 1)
            return;
        curCheckpoint = -1;
        LoadLevel(curSceneIndex - 1);
    }
    public void ReloadLevel()
    {
        LoadLevel(curSceneIndex);
    }
    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("No scene " + sceneIndex);
            return;
        }
        if (sceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            endGameScreen.Show();
            // play sfx?
            return;
        }
        Debug.Log("Loading scene " + sceneIndex);
        if (curSceneIndex != 0 || curSceneIndex == sceneIndex)
        {
            SceneManager.UnloadSceneAsync(curSceneIndex);
        }
        if (sceneIndex != 0)
        {
            StartCoroutine(LoadAndWait(sceneIndex));
        }
    }
    IEnumerator LoadAndWait(int sceneIndex)
    {
        loadingScreen.Show();
        Time.timeScale = 0;
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
            if (loadingSlider) loadingSlider.value = loading.progress;
        }
        curSceneIndex = sceneIndex;
        player.Respawn();
        yield return null;
        float waitTime = minLoadingTime;
        while (waitTime > 0)
        {
            waitTime -= Time.unscaledDeltaTime;
            if (loadingSlider) loadingSlider.value = minLoadingTime - waitTime;
            yield return null;
        }
        Time.timeScale = 1;
        loadingScreen.Hide();
    }
    public void ActivateCheckpoint(int index)
    {
        curCheckpoint = index;
        curSceneIndex = SceneManager.GetSceneAt(1).buildIndex;
        player.ActivatedCheckpoint();
    }
    /// save player progress to playerprefs
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Level", curSceneIndex);
        PlayerPrefs.SetInt("Checkpoint", curCheckpoint);
        // anything else?
    }
    public void LoadSavedGame()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            curSceneIndex = PlayerPrefs.GetInt("Level");
            curCheckpoint = PlayerPrefs.GetInt("Checkpoint");
            ReloadLevel();
        }
        else
        {
            Debug.Log("No saved data!");
            curCheckpoint = 0;
            LoadLevel(1);
        }
    }
    /// returns the spawn position of the current checkpoint of the current level
    public Transform GetCheckpoint()
    {
        if (!GameObject.FindGameObjectWithTag("Level"))
        {
            if (curSceneIndex != 0)
            {
                Debug.LogWarning("No Checkpoint!");
            }
            return player.transform;
        }
        Level curLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
        if (curCheckpoint >= curLevel.checkpoints.Count)
        {
            Debug.Log("Loaded incorrect checkpoint! " + curCheckpoint);
            return null;
        }
        else if (curCheckpoint < 0)
        {
            curCheckpoint = curLevel.checkpoints.Count - 1;
        }
        return curLevel.checkpoints[curCheckpoint].spawnPosition;
    }
    public void MoveToNextCheckpoint()
    {
        curCheckpoint++;
        player.Respawn();
    }
}
