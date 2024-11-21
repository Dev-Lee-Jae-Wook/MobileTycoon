using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using System.Diagnostics;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class AuctionWait : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;

		public AuctionWait(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_AuctionWait;

		public void Enter()
		{
			_auction.CustomerReady(owner);
		}

		public FSMStateType Excute()
		{ 
			return Type;
		}

		public void Exit()
		{
		}
	}
}