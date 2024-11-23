using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.Upgrad
{
    public class UpgradAreaUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private Canvas _popup;
		[SerializeField] private Slider _progress;
		[SerializeField] private TMP_Text _platformLabel;
		[SerializeField] private TMP_Text _money;
		[SerializeField] private TMP_Text _info;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			UpgradArea moneyArea = GetComponent<UpgradArea>();
			moneyArea.OnUpdateMoney += UpdataProgress;
			moneyArea.OnSetupTargetMoney += SetUpProgress;
			moneyArea.OnPlayerDown += () => PopupToggle(true);
			moneyArea.OnPlayerUp += () => PopupToggle(false);
			moneyArea.OnMax += Max;
			PopupToggle(false);
		}


		#endregion

		#region Private Method
		private void Max()
		{
			_progress.maxValue = 1;
			_progress.value = 1;
			_money.text = "MAX";
			_info.text = $"{_platformLabel.text}\nLV : MAX";
		}
		private void UpdataProgress(int money)
		{
			_progress.value = _progress.maxValue - money;
			_money.text = money.ToString();
		}

		private void SetUpProgress(string name, int maxValue)
		{
			_platformLabel.text = name;
			_info.text = $"{name}";
			_progress.value = 0;
			_progress.maxValue = maxValue;
			_money.text = maxValue.ToString();
		}

		private void PopupToggle(bool isVisable)
		{
			_popup.enabled = isVisable;
		}
		#endregion
	}
}
