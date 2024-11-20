using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EverythingStore.AuctionSystem
{
	public class AuctionParticipant
	{
		#region Field
		private AuctionSubmit _submit;
		private int _money;
		private float _priority;
		#endregion

		#region Property
		public int Money => _money;
		#endregion

		#region Event
		#endregion

		#region Public Method
		public AuctionParticipant(AuctionSubmit submit, int money, float priority = 1.0f)
		{
			_submit = submit;
			_money = money;
			_priority = priority;
		}

		public bool CanSubmit()
		{
			return _submit.GetMinimumBidMoney() <= _money;
		}

		/// <summary>
		/// 입찰을 시도합니다.
		/// </summary>
		public bool TrySubmit()
		{
			int minimun = _submit.GetMinimumBidMoney();
			
			if(_money <  minimun)
			{
				return false;
			}
			
			//입찰 우선도 참여를 결정
			if (GetSubmit() == false)
			{
				return false;
			}

			_submit.Submit(minimun, this);

			return true;
		}

		private bool GetSubmit()
		{
			return Random.Range(0.0f, 1.0f) <= _priority;
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}