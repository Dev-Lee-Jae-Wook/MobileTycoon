using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class MoveToTable : CustomerAuctionStateBase, IFSMState
	{
		private NavmeshMove _move;
		private Vector3 _movePoint;
		private bool _isArrived;

		public MoveToTable(CustomerAuction owner, NavmeshMove move, Vector3 movePoint) : base(owner)
		{
			_move = move;
			_movePoint = movePoint;
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_MoveTo_Table;

		public void Enter()
		{
			_isArrived = false;
			owner.Situp(true);
			_move.MovePoint(_movePoint, Arrive);
		}

		public FSMStateType Excute()
		{
			if(_isArrived == true)
			{
				return FSMStateType.CustomerAuction_Reaction_SucessAuctionItem;
			}

			return Type;
		}

		public void Exit()
		{
		}

		private void Arrive()
		{
			_isArrived = true;
		}
	}
}