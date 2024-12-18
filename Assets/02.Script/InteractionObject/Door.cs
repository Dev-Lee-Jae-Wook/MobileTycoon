using EverythingStore.AI.CustomerStateAuction;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class Door : MonoBehaviour, ISwitch
	{
		#region Field
		[SerializeField] Animator _animator;
		private bool _isOpen = false;




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
			_isOpen = !_isOpen;
			_animator.SetBool("IsOpen", _isOpen);
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion



	}
}