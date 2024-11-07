using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerState
{
	public class CustomerStateOrgine : CustomerStateBase, IFSMState
	{
		public CustomerStateOrgine(Customer owner) : base(owner)
		{
		}

		public FSMStateType Type => throw new System.NotImplementedException();

		public void Enter()
		{
			throw new System.NotImplementedException();
		}

		public FSMStateType Excute()
		{
			throw new System.NotImplementedException();
		}

		public void Exit()
		{
			throw new System.NotImplementedException();
		}
	}
}