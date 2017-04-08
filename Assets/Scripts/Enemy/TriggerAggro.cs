using UnityEngine;

public class TriggerAggro : MonoBehaviour {

	public GameObject[] targets;
	public bool disableAfter = true;
	
	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Player"))
			return;
		foreach (GameObject target in targets)
		{
			if (target)
				target.SendMessage("Aggro");
		}
		if (disableAfter)
			gameObject.SetActive(false);
	}
}
