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
			_move.MovePoint(_auction.PickupPoint, PickupAuctionItem);
			_auction.OnPickUpAuctionItem += Finsh;
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
			_auction.OnPickUpAuctionItem -= Finsh;
		}

		public void PickupAuctionItem()
		{
			_auction.PickupAuctionItem(_pickup);
		}

		private void Finsh()
		{
			_isFinsh = true;
		}
	}
}