using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.AI.CustomerState
{
	public  class GoToOutSide : CustomerStateBase, IFSMState
	{
		#region Filde
		private CustomerMove _move;
		private Vector3 _exitPoint;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_GoToOutSide;
		#endregion

		#region Public Method
		public GoToOutSide(Customer owner, Vector3 exitPoint) : base(owner)
		{
			_move = owner.Move;
			_exitPoint = exitPoint;
		}

		public void Enter()
		{
			_move.MovePoint(_exitPoint);
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;

			return next;
		}

		public void Exit()
		{
		}
		#endregion

	}
}
