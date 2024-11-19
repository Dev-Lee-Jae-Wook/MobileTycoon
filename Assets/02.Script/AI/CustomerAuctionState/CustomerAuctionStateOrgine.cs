using EverythingStore.Actor.Customer;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class CustomerAuctionStateOrgine : CustomerAuctionStateBase, IFSMState
	{
		public CustomerAuctionStateOrgine(CustomerAuction owner) : base(owner)
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