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
		#endregion

		#region Property
		public bool isContorl { get; set; }
		#endregion

		#region UnityCycle
		private void Update()
		{
			Vector3 dir = _joyStick.Direction;

			Quaternion r = Quaternion.Euler(30.0f, 0f, 0f);

			Vector3 newDir = Quaternion.Euler(0f, 0f, -30f) * dir;

			_playerMovement.MovementUpdate(newDir);
		}
		#endregion

	}
}