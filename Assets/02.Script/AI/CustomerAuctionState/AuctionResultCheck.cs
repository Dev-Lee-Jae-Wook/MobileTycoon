using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class AuctionResultCheck : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;

		public AuctionResultCheck(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_ResultCheck;

		public void Enter()
		{
		}

		public FSMStateType Excute()
		{
			if(_auction.IsBidder(owner.Participant) == true)
			{
				return FSMStateType.CustomerAuction_SuccesBid;
			}
			else
			{
				Debug.Log("Fail");
				return FSMStateType.CustomerAuction_FailBid;
			}
		}

		public void Exit()
		{
		}
	}
}