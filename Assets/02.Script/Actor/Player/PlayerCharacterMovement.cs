using EverythingStore.Animation;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace EverythingStore.Actor.Player
{
	[RequireComponent(typeof(CharacterController))]
    public class PlayerCharacterMovement : MonoBehaviour, IAnimationEventMovement
    {
		#region Field

        [SerializeField]	private float _rotationSpeed;
        private CharacterController _characterController;

        private float _targetAngle;
		private float _currentVelocity;

		#endregion

		#region Event
		public event Action<float> OnAnimationMovement;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}
		#endregion

		#region Property
		[field: SerializeField] public float Speed { get; set; }
		#endregion

		#region Public Method
		/// <summary>
		/// 입력에 따라 움직임을 갱신합니다.
		/// </summary>
		/// <param name="inputDir">입력된 방향 벡터</param>
		public void MovementUpdate(Vector2 inputDir)
		{
			//움직일 떄
			if(inputDir.sqrMagnitude > 0.0f)
			{
				RotationUpdate(inputDir);
				Move();
				OnAnimationMovement?.Invoke(1.0f);
			}
			//멈출 때
			else
			{
				OnAnimationMovement?.Invoke(0.0f);
			}
		}
		#endregion

		#region Private
		/// <summary>
		/// 캐릭터가 바라보는 방향으로 이동합니다.
		/// </summary>
		private void Move()
		{
			Vector3 forward = transform.forward;
			_characterController.SimpleMove(forward * Speed);
		}

		/// <summary>
		/// 입력된 값에 맞추어서 캐릭터를 부드럽게 회전 시킵니다.
		/// </summary>
		private void RotationUpdate(Vector2 dir)
		{
			_targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

			var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _currentVelocity, _rotationSpeed);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		#endregion Method
	}
}
