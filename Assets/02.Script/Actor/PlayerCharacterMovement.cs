using EverythingStore.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor
{
    public class PlayerCharacterMovement : MonoBehaviour, IAnimationEventMovement
    {
		#region Field

		[SerializeField]	private float _speed;
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



		#region Method
		#region Public
		/// <summary>
		/// �Է¿� ���� �������� �����մϴ�.
		/// </summary>
		/// <param name="inputDir">�Էµ� ���� ����</param>
		public void MovementUpdate(Vector2 inputDir)
		{
			//������ ��
			if(inputDir.sqrMagnitude > 0.0f)
			{
				RotationUpdate(inputDir);
				Move();
				OnAnimationMovement?.Invoke(1.0f);
			}
			//���� ��
			else
			{
				OnAnimationMovement?.Invoke(0.0f);
			}
		}
		#endregion

		#region Private
		/// <summary>
		/// ĳ���Ͱ� �ٶ󺸴� �������� �̵��մϴ�.
		/// </summary>
		private void Move()
		{
			Vector3 forward = transform.forward;
			_characterController.SimpleMove(forward * _speed);
		}

		/// <summary>
		/// �Էµ� ���� ���߾ ĳ���͸� �ε巴�� ȸ�� ��ŵ�ϴ�.
		/// </summary>
		private void RotationUpdate(Vector2 dir)
		{
			_targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

			var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _currentVelocity, _rotationSpeed);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		#endregion

		#endregion
	}
}
