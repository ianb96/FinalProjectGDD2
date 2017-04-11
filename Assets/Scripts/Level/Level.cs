using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

/// manages the individual components of this level
public class Level : MonoBehaviour {

	public List<Checkpoint> checkpoints = new List<Checkpoint>();
	LevelManager lm;

	void Awake() {
		if (!GameObject.FindGameObjectWithTag("LevelManager"))
		{
			Debug.LogError("Add main scene to hierarchy to test");
		}
        lm = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
	}
	void Start()
	{
		Debug.ClearDeveloperConsole();
		checkpoints[0].levelBoundary.SetActive(true);
		for (int i = 0; i < checkpoints.Count; i++)
		{
			checkpoints[i].SetCheckpointIndex(i);
		}
	}
	// deactivate other checkpoints when another is activated ?
}
