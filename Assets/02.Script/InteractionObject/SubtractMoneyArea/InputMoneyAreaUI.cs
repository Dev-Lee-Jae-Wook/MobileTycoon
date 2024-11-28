using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.Upgrad
{
    public class InputMoneyAreaUI : MonoBehaviour
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
		private void Awake()
		{
			InputMoneyArea moneyArea = GetComponent<InputMoneyArea>();
			moneyArea.OnUpdateMoney += UpdataProgress;
			moneyArea.OnSetUp += SetUp;
		}

		private void OnValidate()
		{
			if(_platformLabel == null)
			{
				return;
			}

			_platformLabel.text = _name;
		}

		#endregion

		#region Private Method
		private void UpdataProgress(int money)
		{
			_progress.value = _progress.maxValue - money;
			_money.text = money.ToString();
		}

		private void SetUp(int targetMoney)
		{
			_money.text = targetMoney.ToString();
			_progress.maxValue = targetMoney;
			_progress.value = 0f;
		}
		#endregion
	}
}
