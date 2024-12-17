using EverythingStore.UI.PopUp;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BottomUpPopup : PopupBase
{
	#region Field
	private Animator _animator;
	#endregion

	#region Property
	#endregion

	#region Event
	public event Action OnClose;
	#endregion

	#region UnityCycle
	protected override void Awake_Initailze()
	{
		_animator = GetComponent<Animator>();
	}

	protected override void Start_Intilaize() { }
	#endregion

	#region Public Method
	public override void Popup()
	{
		base.Popup();
		_animator.SetTrigger("PopUp");
	}
	#endregion


	#region Protected Method
	protected override void CloseButtonAction()
	{
		_animator.SetTrigger("PopDown");
	}
	#endregion
}
