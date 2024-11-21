using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class AuctionResultCheck : CustomerAuctionStateBase, IFSMState
	{

		public AuctionResultCheck(CustomerAuction owner) : base(owner)
		{
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_ResultCheck;

		public void Enter()
		{
		}

		public FSMStateType Excute()
		{
			if(owner.IsAuctionSucess == true)
			{
				return FSMStateType.CustomerAuction_SuccesBid;
			}
			else
			{
				Debug.Log("Fail");
				return FSMStateType.CustomerAuction_FailBid;
			}
		}

		public void Exit()
		{
		}
	}
}