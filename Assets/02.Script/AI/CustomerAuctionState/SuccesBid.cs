using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class SuccesBid : CustomerAuctionStateBase, IFSMState
	{
		private NavmeshMove _move;
		private Auction _auction;
		private PickupAndDrop _pickup;
		private bool _isFinsh;
		private PickableObject _auctionItem;

		public SuccesBid(CustomerAuction owner, Auction auction,NavmeshMove move) : base(owner)
		{
			_move = move;
			_auction = auction;
			_pickup = owner.pickupAndDrop;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_SuccesBid;

		public void Enter()
		{
			_isFinsh = false;
			owner.Situp(true);
			_auctionItem = _auction.AuctionItem;
			_move.MovePoint(_auction.PickupPoint, PickupAuctionItem);
		}

		public FSMStateType Excute()
		{
			if(_isFinsh == true)
			{
				return FSMStateType.CustomerAuction_MoveToExit;
			}

			return Type;
		}

		public void Exit()
		{
			
		}

		public void PickupAuctionItem()
		{
			_pickup.Pickup(_auctionItem, Finsh);
			_auction.SucessBidExit();
		}

		private void Finsh()
		{
			_isFinsh = true;
		}
	}
}