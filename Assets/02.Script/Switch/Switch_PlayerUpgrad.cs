using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using UnityEngine;

namespace EverythingStore.Switch
{
	public class Switch_PlayerUpgrad : MonoBehaviour, ISwitch
	{
		#region Field
		[SerializeField] private UpgradPlayer _upgrad;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void SwitchAction()
		{
			_upgrad.Popup();
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion



	}
}
