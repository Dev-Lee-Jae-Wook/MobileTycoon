using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Sensor;

namespace EverythingStore.AI.CustomerState
{
	public  class TriggerWait : CustomerStateBase, IFSMState,ITrigger
	{
		#region Field
		private bool _isTrigger;
		private FSMStateType _nextState;
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Customer_TriggerWait;
		#endregion

		#region Public Method
		public TriggerWait(Customer owner, FSMStateType nextState) : base(owner)
		{
			_nextState = nextState;
		}

		public void Enter()
		{
			_isTrigger = false;
		}

		public FSMStateType Excute()
		{
			FSMStateType next = Type;
			if(_isTrigger == true)
			{
				next = _nextState;
			}

			return next;
		}

		public void Exit()
		{
		}

		public void OnTrigger()
		{
			_isTrigger = true;
		}
		#endregion

	}
}
