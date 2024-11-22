using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class TakeAuctionItem : CustomerAuctionStateBase, IFSMState
	{
		private NavmeshMove _move;
		private Auction _auction;
		private PickupAndDrop _pickup;
		private bool _isTakeAuctionItem;
		private float _waitTime;

		public TakeAuctionItem(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
			_pickup = owner.pickupAndDrop;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_TakeAuctionItem;

		public void Enter()
		{
			_isTakeAuctionItem = false;
			_auction.OnPickUpAuctionItem += Finsh;
			_auction.PickupAuctionItem(_pickup);
		}

		public FSMStateType Excute()
		{
			if(_isTakeAuctionItem == true)
			{
				return FSMStateType.CustomerAuction_MoveTo_MidPoint;
			}

			return Type;
		}

		public void Exit()
		{
			_auction.OnPickUpAuctionItem -= Finsh;
		}

		private void Finsh()
		{
			_isTakeAuctionItem = true;
		}
	}
}