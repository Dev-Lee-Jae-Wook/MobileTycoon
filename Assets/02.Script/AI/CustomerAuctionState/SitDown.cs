using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using System;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class SitDown : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;
		private NavmeshMove _move;
		private bool _isArrive;
		private int _customerIndex;

		public SitDown(CustomerAuction owner, Auction auction, NavmeshMove move) : base(owner)
		{
			_auction = auction;
			_move = move;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_Sitdown;

		public void Enter()
		{
			_isArrive = false;
			_auction.RegisterCustomer(owner);
			_move.MovePoint(owner.Chair.GetEnterPoint(), Arrive);
		}

		

		public FSMStateType Excute()
		{
			FSMStateType type = Type;

			if(_isArrive == true)
			{
				owner.Sitdown();
				type = FSMStateType.CustomerAuction_AuctionWait;
			}

			return type;
		}

		public void Exit()
		{
		}

		private void Arrive()
		{
			_isArrive = true;
		}
	}
}