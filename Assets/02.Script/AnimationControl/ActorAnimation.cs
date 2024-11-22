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

			if (TryGetComponent<IAuctionCustomerAnimationEventAction>(out var action))
			{
				action.OnAnimationSitdown += SitDown;
				action.OnAnimationSitup += SitUp;
				action.OnAnimationSittingClap += SittingClap;
				action.OnAnimationRaising += Raising;
				action.OnAnimationReactionEnd += ReactionEnd;
				action.OnAnimationReactionFail += FailReaction;
				action.OnAnimationReactionSucess += SucessReaction;
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
			_animator.SetLayerWeight(1, 1.0f);
		}		
		private void SitUp()
		{
			_animator.SetTrigger("Situp");
			_animator.SetLayerWeight(1, 0.0f);
		}

		private void SittingClap()
		{
			_animator.SetTrigger("SittingClap");
		}

		private void FailReaction()
		{
			_animator.SetTrigger("FailReaction");
		}

		private void Raising()
		{
			_animator.SetTrigger("Raising");
		}

		private void ReactionEnd()
		{
			_animator.SetTrigger("ReactionEnd");
		}

		private void SucessReaction()
		{
			_animator.SetTrigger("SucessReaction");
		}
	}
}
