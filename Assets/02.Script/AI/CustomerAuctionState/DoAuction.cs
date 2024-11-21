using EverythingStore.Actor.Customer;
using EverythingStore.AuctionSystem;
using EverythingStore.InteractionObject;
using UnityEngine;

namespace EverythingStore.AI.CustomerStateAuction
{
	public class DoAuction : CustomerAuctionStateBase, IFSMState
	{
		private Auction _auction;
		private AuctionParticipant _participant;
		private bool _isRunAuction;
		private float _randomCoolTime;

		public DoAuction(CustomerAuction owner, Auction auction) : base(owner)
		{
			_auction = auction;
			_participant = owner.Participant;
			SetRandomCoolTime();
		}

		public FSMStateType Type => FSMStateType.CustomerAuction_DoAcution;

		public void Enter()
		{
			_isRunAuction = true;
			_auction.OnFinshAuction += FinshAuction;
		}

		public FSMStateType Excute()
		{
			if(owner.IsAuctionResult == true)
			{
				return FSMStateType.CustomerAuction_ResultCheck;
			}

			if(_randomCoolTime <= 0.0f)
			{
				if(_participant.TrySubmit() == true)
				{
					owner.Raising();
				}

				SetRandomCoolTime();
			}
			else
			{
				_randomCoolTime -= Time.deltaTime;
			}

            return Type;
		}

		public void Exit()
		{
			_auction.OnFinshAuction -= FinshAuction;
		}

		private void FinshAuction()
		{
			_isRunAuction = false;
		}

		private void SetRandomCoolTime()
		{
			_randomCoolTime = Random.Range(0.0f, 1.0f);
		}
	}
}