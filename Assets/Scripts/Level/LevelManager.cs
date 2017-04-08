using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Manages the current scene
public class LevelManager : MonoBehaviour
{

    public int loadSceneImmediately = -1;
    public Screen loadingScreen;
    int curSceneIndex = 0;
    int curCheckpoint = 0;
    Player player;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        if (loadSceneImmediately>0)
            LoadLevel(loadSceneImmediately);
    }

    public void NextLevel()
    {
        LoadLevel(curSceneIndex + 1);
    }
    public void ReloadLevel()
    {
        LoadLevel(curSceneIndex);
    }
    public void LoadLevel(int index)
    {
        if (index > SceneManager.sceneCount)
        {
            Debug.LogWarning("No scene " + index);
            return;
        }
        Debug.Log("Loading scene " + index);
        if (curSceneIndex != 0)
        {
            SceneManager.UnloadSceneAsync(curSceneIndex);
        }
        if (index != 0)
        {
            StartCoroutine(LoadAndWait(index));
        }
    }
    IEnumerator LoadAndWait(int index)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
            //loading.progress
        }
        curSceneIndex = index;
        player.Respawn();
    }
    public void ActivateCheckpoint(int index)
    {
        curCheckpoint = index;
        player.ActivatedCheckpoint();
    }
    /// save player progress to playerprefs
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Level", curSceneIndex);
        PlayerPrefs.SetInt("Checkpoint", curCheckpoint);
        // anything else?
    }
    /// returns the spawn position of the current checkpoint of the current level
    public Transform GetCheckpoint()
    {
        Level curLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
        if (curLevel.checkpoints.Count <= curCheckpoint)
        {
            Debug.LogError("Loaded incorrect checkpoint! " + curCheckpoint);
        }
        return curLevel.checkpoints[curCheckpoint].spawnPosition;
    }
}
