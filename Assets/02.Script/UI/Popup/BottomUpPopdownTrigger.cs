using UnityEngine;

namespace EverythingStore.UI.PopUp
{
	public class BottomUpPopdownTrigger : MonoBehaviour
	{
		private BottomUpPopup _popup;

		private void Awake()
		{
			_popup = GetComponent<BottomUpPopup>();
		}

		public void PopdownTrigger()
		{
			_popup.Popdown();
		}
	}
}
