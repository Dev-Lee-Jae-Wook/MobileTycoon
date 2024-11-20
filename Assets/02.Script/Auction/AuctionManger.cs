using EverythingStore.Manger;
using System;
using System.Collections;
using UnityEngine;

namespace EverythingStore.AuctionSystem
{
	public enum AuctionState
	{
		Close,
		WaitAuctionItem,
		Open,
		Wait,
		Start,
		SuccessfulBid,
	}

	public class AuctionManger : MonoBehaviour
	{
		#region Field
		[SerializeField] private CustomerManager _customerManager;
		private AuctionParticipant _lastBid;
		private int _bidMoney;
		private AuctionSubmit _submit;
		private AuctionParticipant[] _participants;
		private float _waitTime;
		private AuctionState _state;
		#endregion

		#region Property
		public int BidMoney => _bidMoney;
		public AuctionSubmit Submit => _submit;
		public AuctionParticipant LastBid => _lastBid;
		public AuctionState State => _state;
		#endregion

		#region Event
		public event Action<int> OnUpdateBidMoney;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_submit = new(this);
		}
		#endregion

		#region Public Method
		public void StartAuction(int bidMoney, Action<AuctionParticipant> callback)
		{
			_state = AuctionState.Start;
			_bidMoney = bidMoney;
			_submit.SetSubmitMinimumMoney(10);
			StartCoroutine(C_Auction(callback));
			_waitTime = 5.0f;
		}

		public void Bid(int money, AuctionParticipant participant)
		{
			_bidMoney = money;
			_lastBid = participant;
			_waitTime = 1.0f;
			OnUpdateBidMoney?.Invoke(_bidMoney);
		}

		public void SetStartBidMoney(int money)
		{
			_bidMoney = money;
		}

		public void WaitAuctionItem()
		{
			_state = AuctionState.WaitAuctionItem;
		}
		#endregion

		#region Private Method
		private IEnumerator C_Auction(Action<AuctionParticipant> callback)
		{
			_waitTime = 1.0f;
			while (_waitTime > 0.0f)
			{
				yield return null;
				_waitTime -= Time.deltaTime;
			}
			callback?.Invoke(_lastBid);
		}

		public void OpenAuction()
		{
			_state = AuctionState.Open;
			_customerManager.SpawnAuctionCustomer();
		}
		#endregion

	}
}
