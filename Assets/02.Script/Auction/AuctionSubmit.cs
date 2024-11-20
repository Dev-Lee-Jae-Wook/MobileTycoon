using EverythingStore.Actor.Customer;

namespace EverythingStore.AuctionSystem
{
	public class AuctionSubmit
	{
		#region Field
		private AuctionManger _manager;
		private int _SubmitMinimumMoney;
		#endregion

		#region Property
		public int SubmitMinimumMoney => _SubmitMinimumMoney;
		#endregion

		#region Event
		#endregion

		#region Public Method
		public AuctionSubmit(AuctionManger manager)
		{
			_manager = manager;
		}

		public void Submit(int money, AuctionParticipant participant)
		{
			_manager.Bid(money, participant);
		}

		public int GetMinimumBidMoney()
		{
			return _manager.BidMoney + _SubmitMinimumMoney;
		}

		public void SetSubmitMinimumMoney(int money)
		{
			_SubmitMinimumMoney = money;
		}

		public void AddSubmitMinimumMoney(int money)
		{
			_SubmitMinimumMoney += money;	
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}