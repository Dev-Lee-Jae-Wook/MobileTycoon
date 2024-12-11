using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InputMoney
{
    public class UnlockSystem : MonoBehaviour
    {
		#region Field
		[SerializeField] private int _targetMoney;
		[SerializeField] private GameObject _targetObject;
		private InputMoneyArea _inputMoneyArea;
		#endregion

		#region Property
		public InputMoneyArea InputMoneyArea => _inputMoneyArea;
		#endregion

		#region Event
		public event Action OnUnlock;
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void Initialization(bool isUnlock , int progressMoney)
		{
			_inputMoneyArea = GetComponent<InputMoneyArea>();
			_inputMoneyArea.Initialize(_targetMoney, progressMoney);
			_inputMoneyArea.OnCompelte += ()=> OnUnlock?.Invoke();
			OnUnlock += () =>
			{
				gameObject.SetActive(false);
				_targetObject.gameObject.SetActive(true);
			};
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion

	}
}
