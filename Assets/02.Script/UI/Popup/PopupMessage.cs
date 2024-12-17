using EverythingStore.UI.PopUp;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EverythingStore.Popup
{
	public class PopupMessage : PopupBase
	{
		#region Field
		[SerializeField] private TMP_Text _title;
		[SerializeField] private TMP_Text _context;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		protected override void Awake_Initailze()
		{
		}

		protected override void Start_Intilaize()
		{
		}

		#endregion

		#region Public Method
		public void SendMessage(string title, string context)
		{
			_title.text = title;
			_context.text = context;
			Popup();
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		protected override void CloseButtonAction()
		{
			Popdown();
		}
		#endregion
	}
}
