using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerState
{
	public class ExitStore : CustomerStateBase, IFSMState
	{
		public ExitStore(Customer owner) : base(owner)
		{
		}

		public FSMStateType Type => FSMStateType.Customer_ExitStore;

		public void Enter()
		{
			owner.Exit();
		}

		public FSMStateType Excute()
		{
			return Type;
		}

		public void Exit()
		{ 
		}
	}
}