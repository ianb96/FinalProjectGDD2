using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public bool follow = true;
	public Transform target;
	public float groundDetachHeight = 4;
	public float vOffset = 2;
	[HideInInspector]
	public float lastGroundHeight;
	bool isShaking = false;
	int levelLayer;

	void Start () {
		levelLayer = 1 << LayerMask.NameToLayer("Level");
		lastGroundHeight = target.transform.position.y;
	}
	
	void Update () {
		if (!follow)
			return;
		if (isShaking)
			return;
		// move camera
        RaycastHit2D downhit = Physics2D.Raycast(target.transform.position, Vector3.down, groundDetachHeight, levelLayer);
        float nCamPosy = target.transform.position.y + vOffset;
        if (downhit.collider)
        {
            nCamPosy = Mathf.Lerp(transform.position.y, lastGroundHeight + vOffset, 20 * Time.deltaTime);
        }
        transform.position = new Vector3(target.transform.position.x, nCamPosy, -10);
	}
	public void StartCameraShake(float magnitude = 0.25f, float duration = 0.25f)
	{
		StartCoroutine(CameraShake(magnitude, duration));
	}
	IEnumerator CameraShake(float magnitude, float duration)
	{
		isShaking = true;
		float progress = 0;
		Vector3 origPos = transform.position;
		while(progress < 1)
		{
			progress += Time.deltaTime / duration;

			// after 75% completion, dampen the effect
			float dampen = 1.0f - Mathf.Clamp(4*progress-3, 0, 1);
			float offX = Random.value * 2.0f - 1.0f;
			float offY = Random.value * 2.0f - 1.0f;
			offX *= magnitude * dampen;
			offY *= magnitude * dampen;

			transform.position = origPos + new Vector3(offX, offY, 0);

			yield return null;
		}
		isShaking = false;
	}
}
