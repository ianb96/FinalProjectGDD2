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
		// reset offset
		GetComponent<RectTransform>().offsetMin = new Vector2(0,0);
		GetComponent<RectTransform>().offsetMax = new Vector2(1,1);
		Hide();
	}

	/// Only screen manager should call this
	public void SMShow()
	{
		cg.interactable = isInteractable;
		cg.blocksRaycasts = isInteractable;
		if (isInteractable)
		{
			Time.timeScale = 0;
		}
		if (anim)
			anim.SetBool("Open", true);
		else
			cg.alpha = 1;
	}
	/// Hide this screen
	public void Hide()
	{
		if (isInteractable)
		{
			Time.timeScale = 1;
		}
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
