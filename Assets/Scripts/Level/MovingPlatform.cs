using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public bool loop = true;
	public bool onlyToToWithTrigger = false;
	public bool onlyToFromWithTrigger = false;
	bool inTrigger = false;
	public Transform fromPos;
	public Transform toPos;
	public float movementSpeed = 3;
	[HideInInspector]
	public Vector3 deltaMovement = Vector3.zero;
	float progressTo = 0;
	public float startOffset = 0;
	float prevProgress = 0;
	float progressTimer = 0;
	void Start () {
		startOffset*=Mathf.PI;
	}
	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		bool downwards = -Mathf.Cos(progressTimer*movementSpeed + startOffset) < 0;
		if (onlyToToWithTrigger && !inTrigger && downwards)
			return;
		if (onlyToFromWithTrigger && !inTrigger && !downwards)
			return;
		progressTimer += Time.deltaTime;
	}
	void FixedUpdate ()
	{
		if (!loop && Mathf.Approximately(progressTo, 1))
			return;
		progressTo = Mathf.Sin(progressTimer*movementSpeed + startOffset)*0.5f+0.5f;
		deltaMovement = -transform.position;
		transform.position = Vector3.Lerp(fromPos.position, toPos.position, progressTo);
		deltaMovement += transform.position;
		prevProgress = progressTo;
	}
	public void ResetProgress()
	{
		Debug.Log("Resetting!");
		progressTo = 0;
		progressTimer = 0;
	}
	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		inTrigger = true;
		//progressTimer = 0;
	}
	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		inTrigger = false;
	}
}
