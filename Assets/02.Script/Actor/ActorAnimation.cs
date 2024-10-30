using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Animation
{
    public class ActorAnimation : MonoBehaviour
    {
		[SerializeField]private Animator _animator;

		private IAnimationEventMovement _movementEvent;
		private IAnimationEventPickupAndDrop _pickupAndDrop;

		private void Awake()
		{
			_animator = GetComponentInChildren<Animator>();
			_movementEvent = GetComponent<IAnimationEventMovement>();
			_pickupAndDrop = GetComponent<IAnimationEventPickupAndDrop>();

			if (_animator == null)
			{
				Debug.LogError($"{gameObject.name} does not have Animator");
			}

			if(_movementEvent != null)
			{
				_movementEvent.OnAnimationMovement += Movement;
			}

			if(_pickupAndDrop != null)
			{
				_pickupAndDrop.OnAnimationPickup += Pickup;
				_pickupAndDrop.OnAnimationDrop += Drop;
			}
		}

		private void Movement(float move)
		{
			_animator.SetFloat("Movement", move);
		}

		private void Pickup()
		{
			_animator.SetTrigger("Pickup");
		}

		private void Drop()
		{
			_animator.SetTrigger("Drop");
		}
	}
}
