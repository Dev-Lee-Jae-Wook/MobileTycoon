using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.Upgrad
{
    public class SubtractMoneyAreaUI : MonoBehaviour
    {
		#region Field
		[SerializeField] private Canvas _popup;
		[SerializeField] private Slider _progress;
		[SerializeField] private TMP_Text _money;
		[SerializeField] private TMP_Text _info;
		[SerializeField] private String _infoLabel;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			SubtractMoneyArea moneyArea = GetComponent<SubtractMoneyArea>();
			moneyArea.OnUpdateMoney += UpdataProgress;
			moneyArea.OnSetupTargetMoney += SetUpProgress;
			moneyArea.OnPlayerDown += () => PopupToggle(true);
			moneyArea.OnPlayerUp += () => PopupToggle(false);

			PopupToggle(false);
		}

		#endregion

		#region Private Method
		private void UpdataProgress(int money)
		{
			_progress.value = _progress.maxValue - money;
			_money.text = money.ToString();
		}

		private void SetUpProgress(int lv,int maxValue)
		{
			_info.text = $"{_infoLabel}\nLV : {lv}";
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
