using EverythingStore.Actor.Customer;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EverythingStore.AuctionSystem
{
	public class AuctionParticipant
	{
		#region Field
		private CustomerAuction _owner;
		private AuctionSubmit _submit;
		private int _money;
		private float _priority;
		#endregion

		#region Property
		public int Money => _money;
		public CustomerAuction Owner => _owner;
		#endregion

		#region Event
		#endregion

		#region Public Method
		public AuctionParticipant(CustomerAuction owner,AuctionSubmit submit)
		{
			_owner = owner;
			_submit = submit;
		}

		public void SetUp(int money, float priority)
		{
			_money = money;
			_priority = priority;
		}

		/// <summary>
		/// ������ �õ��մϴ�.
		/// </summary>
		public bool TrySubmit()
		{
			if(_submit.IsLastOrder(this) == true)
			{
				return false;
			}

			int minimun = _submit.GetMinimumBidMoney();
			

			if(_money <  minimun)
			{
				return false;
			}
			
			//���� �켱�� ������ ����
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