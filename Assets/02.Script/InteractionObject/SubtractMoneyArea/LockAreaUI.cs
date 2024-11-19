using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.Upgrad
{
    public class LockAreaUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private string _name;
		[SerializeField] private Slider _progress;
		[SerializeField] private TMP_Text _platformLabel;
		[SerializeField] private TMP_Text _money;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			LockArea moneyArea = GetComponent<LockArea>();
			_platformLabel.text = _name;
			moneyArea.OnUpdateMoney += UpdataProgress;
			_progress.maxValue = moneyArea.MaxMoney;
			_money.text = _progress.maxValue.ToString();
		}


		#endregion

		#region Private Method
		private void UpdataProgress(int money)
		{
			_progress.value = _progress.maxValue - money;
			_money.text = money.ToString();
		}
		#endregion
	}
}
