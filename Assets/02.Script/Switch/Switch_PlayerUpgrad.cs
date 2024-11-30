using EverythingStore.Actor.Player;
using EverythingStore.InteractionObject;
using EverythingStore.Upgrad;
using UnityEngine;

namespace EverythingStore.Switch
{
	public class Switch_PlayerUpgrad : MonoBehaviour, ISwitch
	{
		#region Field
		[SerializeField] private PlayerInput  _input;
		[SerializeField] private UpgradPlayer _upgrad;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_upgrad.OnOpen += () => _input.SetControler(false);
			_upgrad.OnClose += () => _input.SetControler(true);
		}
		#endregion

		#region Public Method
		public void SwitchAction()
		{
			_upgrad.Open();
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion



	}
}
