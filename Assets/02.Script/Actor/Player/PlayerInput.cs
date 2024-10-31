using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Player
{
    public class PlayerInput : MonoBehaviour
    {
		#region Field
		[SerializeField] private PlayerCharacterMovement _playerMovement;
		#endregion

		#region Property
		public bool isContorl { get; set; }
		#endregion

		#region UnityCycle
		private void Update()
		{
			Vector2 dir;
			dir.x = Input.GetAxisRaw("Horizontal");
			dir.y = Input.GetAxisRaw("Vertical");
			dir.Normalize();

			_playerMovement.MovementUpdate(dir);
		}
		#endregion

		public void TestCode()
		{

		}
	}
}