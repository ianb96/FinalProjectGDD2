using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public bool loop = true;
	public Transform fromPos;
	public Transform toPos;
	public float movementSpeed = 3;
	[HideInInspector]
	public Vector3 deltaMovement = Vector3.zero;
	float progressTo = 0;
	public float startOffset = 0;
	void Start () {
		startOffset*=Mathf.PI;
	}
	
	void FixedUpdate () {
		if (!loop && Mathf.Approximately(progressTo, 1))
			return;
		progressTo = Mathf.Sin(Time.timeSinceLevelLoad*movementSpeed + startOffset)*0.5f+0.5f;
		deltaMovement = -transform.position;
		transform.position = Vector3.Lerp(fromPos.position, toPos.position, progressTo);
		deltaMovement += transform.position;
	}
	public void ResetProgress()
	{
		progressTo = 0;
	}
}
