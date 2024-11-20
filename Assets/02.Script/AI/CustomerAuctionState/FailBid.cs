using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class FailBid : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;
		private bool _isPickup;
		private PickupAndDrop _pickupAndDrop;
		private bool isFinsh = false;

		public FailBid(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_FailBid;

		public void Enter()
		{
			isFinsh = false;
			_auction.OnSuccessBidExit += Finsh;
		}

		public FSMStateType Excute()
		{
			if (isFinsh == false)
			{
				return Type;
			}
			else  
			{
				return FSMStateType.CustomerAuction_MoveToExit;
			}
		}

		public void Exit()
		{
			owner.Situp(false);
			_auction.OnSuccessBidExit -= Finsh;
		}

		private void Finsh()
		{
			isFinsh = true;
		}

	}
}