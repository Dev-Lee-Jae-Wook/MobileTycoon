using EverythingStore.Actor.Customer;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class ReactionSucessAuctionItem : CustomerAuctionStateBase, IFSMState
	{
		private enum State
		{
			LookAtCustoerm,
			Reaction,
			LookAtTable,
			Next,
		}

		private float _turnTime = 0.5f;
		private float _time;
		private Quaternion _lookAtCustomer;
		private Quaternion _lookAtTable;

		private State state;
		private float _reactionTime = 3.0f;

		public ReactionSucessAuctionItem(CustomerAuction owner) : base(owner)
		{
			_lookAtCustomer = Quaternion.Euler(0f, -180f, 0f);
			_lookAtTable = Quaternion.Euler(0f, 0f, 0f);
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_Reaction_SucessAuctionItem;

		public void Enter()
		{
			state = State.LookAtCustoerm;
			_time = 0.0f;
		}

		public FSMStateType Excute()
		{
			FSMStateType type = Type;

			switch (state)
			{
				case State.LookAtCustoerm:
					UpdateLookAtCustomer();
					break;
				case State.Reaction:
					UpdateReaction();
					break;
				case State.LookAtTable:
					UpdateLookAtTable();
					break;
				case State.Next:
					type = FSMStateType.CustomerAuction_TakeAuctionItem;
					break;
			}

			return type;
		}

		public void Exit()
		{

		}

		private void UpdateLookAtCustomer()
		{
			_time += Time.deltaTime;
			float progress = _time / _turnTime;
			Debug.Log(progress);
			owner.transform.rotation = Quaternion.Lerp(_lookAtTable, _lookAtCustomer, progress);

			if(_time > _turnTime)
			{
				EnterReaction();
			}
		}

		private void EnterReaction()
		{
			state = State.Reaction;
			_time = 0.0f;
			owner.transform.rotation = _lookAtCustomer;
			owner.ReactionSucessAuctionItem();
		}

		private void UpdateReaction()
		{
			_time += Time.deltaTime;
			if(_time >= _reactionTime)
			{
				EnterLookAtTable();
			}
		}

		private void EnterLookAtTable()
		{
			state = State.LookAtTable;
			_time = 0.0f;
			owner.ReactionEnd();
		}

		private void UpdateLookAtTable()
		{
			_time += Time.deltaTime;
			float progress = _time / _turnTime;
			owner.transform.rotation = Quaternion.Lerp(_lookAtCustomer, _lookAtTable, progress);

			if (_time > _turnTime)
			{
				owner.transform.rotation = _lookAtTable;
				state = State.Next;
			}
		}
	}
}