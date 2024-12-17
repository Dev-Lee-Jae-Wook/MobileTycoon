
using Codice.CM.Client.Differences.Merge;
using EverythingStore.InteractionObject;
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
		/// moeny가 갱신될 때 호출됩니다.
		/// </summary>
		public event Action<int> OnUpdate;

		/// <summary>
		/// Money가 갱신될 때 호출됩니다.
		/// </summary>
		public event Action<string> OnUpdateString;

		public event Action OnAddMoney;
		public event Action OnSubtractMoney;
		#endregion

		#region Public Method

		public void SetMoney(in int money)
		{
			_money = money;
			OnUpdate?.Invoke(_money);
			OnUpdateString?.Invoke(GetFormatSuffix());
			OnAddMoney?.Invoke();
		}

		public void AddMoney(int money)
		{
			_money += money;
			OnUpdate?.Invoke(_money);
			OnUpdateString?.Invoke(GetFormatSuffix());
			OnAddMoney?.Invoke();
		}

		public void SubtractMoney(int money)
		{
			_money -= money;
			OnUpdate?.Invoke(_money);
			OnUpdateString?.Invoke(GetFormatSuffix());
			OnSubtractMoney?.Invoke();
		}

		public bool CanSubstactMoney(int money)
		{
			return _money >= money;
		}

		public string GetFormatSuffix()
		{
			float result;
			string unit = null;

			if (_money >= 1000000)
			{
				result = ((float)_money / 1000000);
				unit = "M";
			}
			else if (_money >= 1000)
			{
				result = ((float)_money / 1000);
				unit = "K";
			}
			else
			{
				result=_money;
			}

			return $"{result:F2}{unit}";
		}
		#endregion

		#region Private Method
		#endregion
	}
}
