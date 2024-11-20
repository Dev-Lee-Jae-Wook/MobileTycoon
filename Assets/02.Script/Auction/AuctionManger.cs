using EverythingStore.Actor.Customer;
using EverythingStore.Sell;
using System;
using UnityEngine;

namespace EverythingStore.AuctionSystem
{
	public class AuctionManger : MonoBehaviour
	{
		#region Field
		private AuctionParticipant _lastBid;
		private SellObject _auctionItem;
		private int _bidMoney;
		private AuctionSubmit _submit;
		#endregion

		#region Property
		public int BidMoney => _bidMoney;
		public AuctionSubmit Submit => _submit;
		public AuctionParticipant LastBid => _lastBid;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_submit = new(this);
		}
		#endregion

		#region Public Method
		public void StartAuction(SellObject sellobject)
		{
			_auctionItem = sellobject;
		}

		public void Bid(int money, AuctionParticipant participant)
		{
			_bidMoney = money;
			_lastBid = participant;
		}

		public void SetStartBidMoney(int money)
		{
			_bidMoney = money;
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}
