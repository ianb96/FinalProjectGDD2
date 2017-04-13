using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// Manages the current scene
public class LevelManager : MonoBehaviour
{

    [ContextMenuItemAttribute("MoveToNextCheckpoint","MoveToNextCheckpoint")]
    public bool RCToNextCP;
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
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Keypad1))
		{
			StartPrevLevel();
		}
        if(Input.GetKeyDown(KeyCode.Keypad2))
		{
			NextLevel();
		}
        if(Input.GetKeyDown(KeyCode.Keypad3))
		{
			MoveToNextCheckpoint();
		}
        if(Input.GetKeyDown(KeyCode.Keypad4))
		{
			ReloadLevel();
		}
	}
    public void NextLevel()
    {
        curCheckpoint = 0;
        LoadLevel(curSceneIndex + 1);
    }
    public void StartPrevLevel()
    {
        if (curSceneIndex<1)
            return;
        curCheckpoint = 0;
        LoadLevel(curSceneIndex - 1);
    }
    public void PrevLevel()
    {
        if (curSceneIndex<1)
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
        Debug.Log("Loading scene " + sceneIndex);
        if (curSceneIndex != 0)
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
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        while (!loading.isDone)
        {
            yield return null;
            //loading.progress
        }
        curSceneIndex = sceneIndex;
        player.Respawn();
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
    /// returns the spawn position of the current checkpoint of the current level
    public Transform GetCheckpoint()
    {
        Level curLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
        if (curCheckpoint >= curLevel.checkpoints.Count)
        {
            Debug.Log("Loaded incorrect checkpoint! " + curCheckpoint);
            return null;
        } else if (curCheckpoint<0) {
            curCheckpoint = curLevel.checkpoints.Count-1;
        }
        return curLevel.checkpoints[curCheckpoint].spawnPosition;
    }
    public void MoveToNextCheckpoint()
    {
        curCheckpoint++;
        player.Respawn();
    }
}
