using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerState
{
	public class MoveToSalesStand : CustomerStateBase, IFSMState
	{
		private NavmeshMove _move;
		private bool _isArrive;

		public MoveToSalesStand(Customer owner, NavmeshMove move) : base(owner)
		{
			_move = move;
		}

		public FSMStateType Type => FSMStateType.Customer_MoveTo_SalesStation;

		public void Enter()
		{
			_isArrive = false;
			_move.MovePoint(owner.EnterSalesStand.InteractionPoint, Arrive);
		}

		public FSMStateType Excute()
		{
			if (_isArrive == true)
			{
				return FSMStateType.Customer_Interaction_SaleStation;
			}
			return Type;
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