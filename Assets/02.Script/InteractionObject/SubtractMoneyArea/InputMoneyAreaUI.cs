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

		private int _targetMoney;
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		private void OnValidate()
		{
			if(_platformLabel == null)
			{
				return;
			}

			_platformLabel.text = _name;
		}

		#endregion

		public void Initialize(InputMoneyArea area, int targetMoney, int progressMoney)
		{
			_targetMoney = targetMoney;
			area.OnUpdateMoney += UpdataProgress;
			_progress.maxValue = targetMoney;
			UpdataProgress(progressMoney);
		}

		#region Private Method
		private void UpdataProgress(int progressMoney)
		{
			_progress.value = progressMoney;
			_money.text = GetMoneyLeft(progressMoney).ToString();
		}

		private int GetMoneyLeft(int money)
		{
			return _targetMoney - money;
		}
		#endregion
	}
}
