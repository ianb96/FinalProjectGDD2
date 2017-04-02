using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {

	Animator anim;
	CanvasGroup cg;

	void Start () {
		cg = GetComponent<CanvasGroup>();
		anim = GetComponent<Animator>();
		Hide();
		// reset offset?
	}
	public void Show()
	{
		cg.interactable = true;
		cg.blocksRaycasts = true;
		if (anim)
			anim.SetBool("Open", true);
		else
			cg.alpha = 1;
	}
	public void Hide()
	{
		cg.interactable = false;
		cg.blocksRaycasts = false;
		if (anim)
			anim.SetBool("Open", false);
		else 
			cg.alpha = 0;
	}
}
