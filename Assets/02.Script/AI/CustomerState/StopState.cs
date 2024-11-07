using EverythingStore.Actor;
using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerState
{
	public  class StopState : CustomerStateBase, IFSMState
	{
		#region Field
		#endregion

		#region Property
		public FSMStateType Type => FSMStateType.Stop;
		#endregion

		#region Public Method
		public StopState(Customer owner) : base(owner)
		{
			owner.GetMachine().StopMachine();
		}

		public void Enter()
		{
			
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
