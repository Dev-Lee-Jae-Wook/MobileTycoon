
using System;

namespace EverythingStore.Actor.Player
{
	public class Wallet
	{
		#region Field
		private int _money;
		#endregion

		#region Property
		public int Money => _money;
		#endregion

		#region Event
		/// <summary>
		/// moeny�� ���ŵ� �� ȣ��˴ϴ�.
		/// </summary>
		public event Action<int> OnUpdate;

		public event Action OnAddMoney;
		public event Action OnSubtractMoney;
		#endregion

		#region Public Method
		public Wallet(int initMoney) 
		{
			_money = initMoney;
		}

		public void AddMoney(int money)
		{
			_money += money;
			OnUpdate?.Invoke(_money);
			OnAddMoney?.Invoke();
		}

		public void SubtractMoney(int money)
		{
			_money -= money;
			OnUpdate?.Invoke(_money);
			OnSubtractMoney?.Invoke();
		}

		public bool CanSubstactMoney(int money)
		{
			return _money >= money;
		}
		#endregion

		#region Private Method
		#endregion
	}
}
