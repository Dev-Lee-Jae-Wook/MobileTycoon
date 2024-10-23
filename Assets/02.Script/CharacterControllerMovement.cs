using EverythingStore.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor
{
    public class CharacterControllerMovement : MonoBehaviour, IAnimationEventMovement
    {
        [SerializeField]
        private float _speed;
        [SerializeField]
		private float _rotationSpeed;

        private CharacterController _characterController;

        private Vector3 _diraction;

        private float _targetAngle;

        private float _currentVelocity;

		public event Action<float> OnMovement;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		public void SetDiraction(Vector3 dir)
        {
			_diraction = dir.normalized;
        }

        // Update is called once per frame
        void Update()
		{
			bool isKeyInput = false;
			Vector2 dir;
			//Test
			dir.x = Input.GetAxisRaw("Horizontal");
			dir.y = Input.GetAxisRaw("Vertical");

			if (dir.sqrMagnitude > 0.0f)
			{
				SetDiraction(dir);
				isKeyInput = true;
			}


			if (isKeyInput == true)
			{
				RotationUpdate();
				Move();
			}
			else
			{
				//멈춘 상태라면 움직임 없다고 알림니다.
				OnMovement?.Invoke(0.0f);
			}
		}

		/// <summary>
		/// 캐릭터를 이동 시킵니다.
		/// </summary>
		/// <param name="isKeyInput"></param>
		private void Move()
		{
			Vector3 forward = transform.forward;
			_characterController.SimpleMove(forward * _speed);
			OnMovement?.Invoke(1.0f);
		}

		/// <summary>
		/// diraction에 맞추어서 캐릭터 회전 시킵니다.
		/// </summary>
		private void RotationUpdate()
		{
			_targetAngle = Mathf.Atan2(_diraction.x, _diraction.y) * Mathf.Rad2Deg;

			var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _currentVelocity, _rotationSpeed);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}



	}
}
