using EverythingStore.Actor.Player;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.UI.PopUp
{
	public abstract class PopupBase : MonoBehaviour
	{
		#region Field
		[SerializeField] private PlayerInput _playerInput;

		[Title("Popup UI")]
		[SerializeField] protected Button closeButton;
		private Canvas _canvas;

		private RectTransform _rectTransfrom;
		#endregion

		#region Event
		public event Action OnPopup;
		public event Action OnPopdown;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_rectTransfrom = GetComponent<RectTransform>();
			_canvas = transform.parent.GetComponent<Canvas>();
			closeButton.onClick.AddListener(CloseButtonAction);

			_rectTransfrom.offsetMin *= Vector2.up;
			_rectTransfrom.offsetMax *= Vector2.up;
			Awake_Initailze();
			OnPopup += ()=> _playerInput.SetControler(false);
			OnPopdown += ()=> _playerInput.SetControler(true);
		}

		private void Start()
		{
			Start_Intilaize();
			_canvas.enabled = false;
			gameObject.SetActive(false);
		}
		#endregion

		#region Public Method
		public virtual void Popup()
		{
			_canvas.enabled = true;
			gameObject.SetActive(true);
			OnPopup?.Invoke();
		}

		public virtual void Popdown()
		{
			_canvas.enabled = false;
			gameObject.SetActive(false);
			OnPopdown?.Invoke();
		}
		#endregion

		#region Protected Method
		protected abstract void CloseButtonAction();
		protected abstract void Awake_Initailze();

		/// <summary>
		/// 초기화 해야되는 것들은 여기에 넣어주세요.
		/// </summary>
		protected abstract void Start_Intilaize();
		#endregion

	}
}