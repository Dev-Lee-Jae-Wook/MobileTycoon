using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PopupUIBase : MonoBehaviour
{
	#region Field
	[Title("Popup UI")]
	[SerializeField] private Canvas _canvas;
	[SerializeField] private Button _closeButton;

	private RectTransform _rectTransfrom;
	private Animator _animator;
	#endregion

	#region Property
	#endregion

	#region Event
	public event Action OnOpen;
	public event Action OnClose;
	#endregion

	#region UnityCycle
	private void Awake()
	{
		_rectTransfrom = GetComponent<RectTransform>();
		_animator = GetComponent<Animator>();
		_closeButton.onClick.AddListener(PopDown);

		_rectTransfrom.offsetMin *= Vector2.up; 
		_rectTransfrom.offsetMax *= Vector2.up;
	}

	private void Start()
	{
		StartInit();
		gameObject.SetActive(false);
		_canvas.enabled = false;
	}
	#endregion

	#region Public Method
	public void Open()
	{
		_canvas.enabled = true;
		OnOpen?.Invoke();
		gameObject.SetActive(true);
		_animator.SetTrigger("PopUp");
	}
	#endregion

	#region Private Method
	private void Close()
	{
		_canvas.enabled = false;
		gameObject.SetActive(false);
		OnClose?.Invoke();
	}
	#endregion

	#region Protected Method

	/// <summary>
	/// 초기화 해야되는 것들은 여기에 넣어주세요.
	/// </summary>
	protected abstract void StartInit();
	protected void PopDown()
	{
		_animator.SetTrigger("PopDown");
	}
	#endregion
}
