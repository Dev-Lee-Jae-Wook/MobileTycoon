using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public class ActorAnimation : MonoBehaviour
    {
		private Animator _animator;

		private IAnimationEventMovement _movementEvent;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_movementEvent = GetComponent<IAnimationEventMovement>();

			if (_animator == null)
			{
				Debug.LogError($"{gameObject.name} does not have Animator");
			}

			if(_movementEvent != null)
			{
				_movementEvent.OnMovement += Movement;
			}
		}

		private void Movement(float move)
		{
			_animator.SetFloat("Movement", move);
		}


	}
}
