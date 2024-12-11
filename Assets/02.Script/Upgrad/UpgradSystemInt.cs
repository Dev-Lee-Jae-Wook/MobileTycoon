using EverythingStore.InteractionObject;
using System;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	public class UpgradSystemInt : MonoBehaviour
	{
		#region Field
		[SerializeField] private UpgradData_Int upgradData;
		private int _lv;
		private int _maxLv;
		private InputMoneyArea _inputMoneyArea;

		private Action<int> _upgradCallback;
		#endregion

		#region Event
		public event Action OnAllComplete;
		public event Action OnUpgrad;
		#endregion

		#region Property
		public InputMoneyArea InputMoneyArea => _inputMoneyArea;
		public int MaxLv => upgradData.GetMaxLv();
		#endregion

		#region Public Method

		public void Inititalize(int lv, int moneyLeft,Action<int> upgradCallBack)
		{
			_lv = lv;
			_maxLv = upgradData.GetMaxLv();
			_upgradCallback = upgradCallBack;

			_inputMoneyArea = GetComponent<InputMoneyArea>();
			_inputMoneyArea.OnCompelte += Upgrad;
			OnAllComplete += () => gameObject.SetActive(false);

			_upgradCallback?.Invoke(GetValue());

			if (IsMaxUpgrad() == true)
			{
				_upgradCallback?.Invoke(upgradData.GetUpgradData(_maxLv).Value);
				OnAllComplete?.Invoke();
			}
			else
			{
				_inputMoneyArea.Initialize(GetNextCost(), moneyLeft);
			}
		}

		public void Upgrad()
		{
			_lv++;
			_upgradCallback?.Invoke(GetValue());
			OnUpgrad?.Invoke();
			if (IsMaxUpgrad() == true)
			{
				OnAllComplete?.Invoke();
			}
			else
			{
				_inputMoneyArea.Initialize(GetNextCost(), 0);
			}
		}
		#endregion

		#region Private Method

		private int GetValue()
		{
			return upgradData.GetUpgradData(_lv).Value;
		}

		private int GetNextCost()
		{
			return upgradData.GetUpgradData(_lv).NextUpgradCost;
		}

		private bool IsMaxUpgrad()
		{
			return _lv == _maxLv;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
