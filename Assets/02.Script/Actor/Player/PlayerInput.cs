using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Player
{
    public class PlayerInput : MonoBehaviour
    {
		#region Field
		[SerializeField] private FixedJoystick _joyStick;
		[SerializeField] private PlayerCharacterMovement _playerMovement;

		private bool _isControl = true;
		#endregion

		#region Property
		#endregion

		#region UnityCycle

		private void Update()
		{
			if(_isControl == false)
			{
				return;
			}

			Vector3 dir = _joyStick.Direction;

			Quaternion r = Quaternion.Euler(30.0f, 0f, 0f);

			Vector3 newDir = Quaternion.Euler(0f, 0f, -30f) * dir;

			_playerMovement.MovementUpdate(newDir);
		}
		#endregion


		#region Public Method
		public void SetControler(bool isControl)
		{
			_isControl = isControl;

			if(_isControl == false)
			{
				_playerMovement.MovementUpdate(Vector2.zero);
			}
		}
		#endregion
	}
}