using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;

namespace EverythingStore.AI.CustomerState
{
	public class EnterSalesStand : CustomerStateBase, IFSMState
	{
		private NavmeshMove _move;
		private IWaitingLine _waitInteraction;
		private bool _isArrive;
		public EnterSalesStand(Customer owner, NavmeshMove move) : base(owner)
		{
			_move = move;
		}

		public FSMStateType Type => FSMStateType.Customer_Enter_SalesStand;

		public void Enter()
		{
			_isArrive = false;
			var enterSalesStand = owner.EnterSalesStand;
			_waitInteraction = enterSalesStand;
			_move.MovePoint(enterSalesStand.EnterPoint, Arrive);
		}

		public FSMStateType Excute()
		{
			if(_isArrive == false)
			{
				return Type;
			}
			else
			{
				return GetNextState();
			}
		}

		public void Exit()
		{
		}

		private void Arrive()
		{
			_isArrive = true;
		}

		private FSMStateType GetNextState()
		{
			if (_waitInteraction.IsWaitingEmpty() == true)
			{
				return  _waitInteraction.EnterInteraction(owner);
			}
			else
			{
				return _waitInteraction.EnterWaitingLine(owner);
			}
		}

	}
}