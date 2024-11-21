using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class Resentful : CustomerAuctionStateBase, IFSMState
	{
		private float _time;

		public Resentful(CustomerAuction owner) : base(owner)
		{
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_Resentful;

		public void Enter()
		{
			_time = 5.0f;
			owner.Resentful();
		}

		public FSMStateType Excute()
		{
			if(_time <= 0.0f)
			{
				return FSMStateType.CustomerAuction_MoveToExit;
			}

			_time -= Time.deltaTime;

			return Type;
		}

		public void Exit()
		{
			owner.ReactionEnd();
		}
	}
}