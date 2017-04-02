using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screen : MonoBehaviour {

	[ContextMenuItemAttribute("Show Screen", "Show")]
	public bool rcToShow;
	[ContextMenuItemAttribute("Hide Screen", "Hide")]
	public bool rcToHide;
	
	Animator anim;
	CanvasGroup cg;
	
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		cg = GetComponent<CanvasGroup>();
		anim = GetComponent<Animator>();
	}

	void Start () {
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
