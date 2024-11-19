using UnityEngine;

namespace EverythingStore.Animation
{
	public class ActorAnimation : MonoBehaviour
	{
		[SerializeField] private Animator _animator;

		private void Awake()
		{
			IAnimationEventMovement movementEvent;
			IAnimationEventPickupAndDrop pickupAndDrop;

			_animator = GetComponentInChildren<Animator>();
			movementEvent = GetComponent<IAnimationEventMovement>();
			pickupAndDrop = GetComponent<IAnimationEventPickupAndDrop>();

			movementEvent.OnAnimationMovement += Movement;
			pickupAndDrop.OnAnimationPickup += Pickup;
			pickupAndDrop.OnAnimationDrop += Drop;

			if (TryGetComponent<IAnimationEventAction>(out var action))
			{
				action.OnAnimationSitdown += SitDown;
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

		private void SitDown()
		{
			_animator.SetTrigger("Sitdown");
		}
	}
}
