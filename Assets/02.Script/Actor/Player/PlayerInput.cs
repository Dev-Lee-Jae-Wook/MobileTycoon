using System;
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
		private bool _isProduct = false;
		#endregion

		#region Event
		public event Action<bool> OnChangeControl;
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
			if(_isProduct == true)
			{
				return;
			}

			_isControl = isControl;

			if(_isControl == false)
			{
				_playerMovement.MovementUpdate(Vector2.zero);
			}

			OnChangeControl?.Invoke(_isControl);
		}

		public void SetProductFixControl(bool isProduct)
		{
            if (isProduct == true)
            {
				SetControler(!isProduct);
				_isProduct = isProduct;
			}
			else
			{
				_isProduct = isProduct;
				SetControler(!isProduct);
			}
		}
		#endregion
	}
}