using EverythingStore.InteractionObject;
using System;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	public class UpgradSystemInt : MonoBehaviour
	{
		#region Field
		[SerializeField] private InputMoneyArea _inputMoneyArea;
		[SerializeField] private UpgradData_Int upgradData;
		private int _lv = 0;
		private IUpgradInt _target;
		#endregion

		#region Event
		public event Action OnAllComplete;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_target = GetComponent<IUpgradInt>();
			_inputMoneyArea.OnCompelte += Upgrad;
			_inputMoneyArea.SetUp(upgradData.UpgradList[_lv].Cost);
		}
		#endregion

		#region Public Method
		public void Upgrad()
		{
			var value = upgradData.UpgradList[_lv].Value;
			_target.Upgrad(value);
			_lv++;
			
			if(CanNextUpgrad() == true)
			{
				int nextCost = upgradData.UpgradList[_lv].Cost;
				_inputMoneyArea.SetUp(nextCost);
			}
			else
			{
				_inputMoneyArea.gameObject.SetActive(false);
				OnAllComplete?.Invoke();
			}
		}
		#endregion

		#region Private Method
		private bool CanNextUpgrad()
		{
			return _lv < upgradData.UpgradList.Count;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
