using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {

	[ContextMenuItemAttribute("Show Screen", "Show")]
	public bool rcToShow;
	[ContextMenuItemAttribute("Hide Screen", "Hide")]
	public bool rcToHide;
	
	public bool isInteractable = false;
	Animator anim;
	CanvasGroup cg;
	
	void Awake()
	{
		cg = GetComponent<CanvasGroup>();
		anim = GetComponent<Animator>();
		Hide();
		// reset offset?
	}

	/// Only screen manager should call this
	public void SMShow()
	{
		cg.interactable = isInteractable;
		cg.blocksRaycasts = isInteractable;
		if (anim)
			anim.SetBool("Open", true);
		else
			cg.alpha = 1;
	}
	/// Hide this screen
	public void Hide()
	{
		cg.interactable = false;
		cg.blocksRaycasts = false;
		if (anim)
			anim.SetBool("Open", false);
		else 
			cg.alpha = 0;
	}
	/// Tells the SceneManager to show this scene
	public void Show()
	{
		GameObject.FindGameObjectWithTag("ScreenManager").GetComponent<ScreenManager>().ShowScreen(this);
	}
}
