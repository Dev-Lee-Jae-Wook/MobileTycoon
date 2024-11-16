using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.AI
{
	public class MoveToPoint : IFSMState
	{
		#region Field
		private NavmeshMove _move;
		private bool _isArrive;
		private FSMStateType _arriveState;
		private FSMStateType _type;
		private Vector3 _arrivePoint;
		#endregion

		#region Property
		public FSMStateType Type => _type;
		#endregion

		#region Public Method
		public MoveToPoint(NavmeshMove move, Vector3 arrivePoint, FSMStateType type, FSMStateType arriveState) 
		{
			_type = type;
			_move = move;
			_arriveState = arriveState;
			_arrivePoint = arrivePoint;
		}

		public void Enter()
		{
			_isArrive = false;
			_move.MovePoint(_arrivePoint, Arrive);
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;
			if(_isArrive == true)
			{
				next = _arriveState;
			}
			return next;
		}

		public void Exit()
		{
		}
		#endregion

		#region Private Method
		private void Arrive()
		{
			_isArrive = true;
		}
		#endregion

	}
}
