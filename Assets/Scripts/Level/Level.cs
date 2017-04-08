using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// manages the individual components of this level
public class Level : MonoBehaviour {

	public List<Checkpoint> checkpoints = new List<Checkpoint>();
	LevelManager lm;

	void Awake() {
		if (!GameObject.FindGameObjectWithTag("LevelManager"))
		{
			StartCoroutine(FixPlayScene());
		}
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	IEnumerator FixPlayScene()
	{
		Debug.ClearDeveloperConsole();
		Scene curScene = SceneManager.GetActiveScene();
		AsyncOperation reloading = SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
		while (!reloading.isDone)
			yield return null;
		lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
		lm.loadSceneImmediately = curScene.buildIndex;
		SceneManager.UnloadSceneAsync(curScene);
	}
	void Start()
	{
		for (int i = 0; i < checkpoints.Count; i++)
		{
			checkpoints[i].SetCheckpointIndex(i);
		}
	}
}
