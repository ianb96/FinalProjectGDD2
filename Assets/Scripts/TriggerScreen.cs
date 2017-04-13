using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScreen : MonoBehaviour {
	public Screen screenToShow;
	public bool hideOnExit = false;
	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			screenToShow.Show();
		}
	}
	/// <summary>
	/// Sent when another object leaves a trigger collider attached to
	/// this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (hideOnExit)
				screenToShow.Hide();
		}
	}
}
