using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	[ContextMenuItemAttribute("Open","Open")]
	[ContextMenuItemAttribute("Close","Close")]
	public bool isOpen = true;
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		anim.SetBool("Open", isOpen);
	}
	public void Open()
	{
		anim.SetBool("Open", true);
	}
	public void Close()
	{
		anim.SetBool("Open", false);
		// make sound?
	}
}
