using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class ReactionFail : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;
		private bool _isPickup;
		private PickupAndDrop _pickupAndDrop;
		private bool isFinsh = false;
		private float _resentfu = 0.4f;

		public ReactionFail(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_Reaction_Fail;

		public void Enter()
		{
			isFinsh = false;
			owner.SittingCrap();
			_auction.OnPickUpAuctionItem += Finsh;
		}

		public FSMStateType Excute()
		{
			if (isFinsh == false)
			{
				return Type;
			}
			else  
			{
				if (Random.Range(0.0f, 1.0f) <= _resentfu)
				{
					return FSMStateType.CustomerAuction_Reaction_Resentful;
				}
				else
				{
					return FSMStateType.CustomerAuction_MoveToExit;
				}
			}
		}

		public void Exit()
		{
			owner.Situp(false);
			_auction.OnPickUpAuctionItem -= Finsh;
		}

		private void Finsh()
		{
			isFinsh = true;
		}

	}
}