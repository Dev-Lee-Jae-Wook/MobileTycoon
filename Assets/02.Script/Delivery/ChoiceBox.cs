using EverythingStore.BoxBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;

namespace EverythingStore.Delivery
{
	public class ChoiceBox : MonoBehaviour
	{
		#region Field
		[SerializeField] private Button _minButton;
		[SerializeField] private Button _maxButton;
		[SerializeField] private Button _okButton;
		[SerializeField] private BoxOrder _boxOrder;
		[SerializeField] private Slider _slider;
		[SerializeField] private TMP_Text _orderAmount;
		[SerializeField] private TMP_Text _context;
		[SerializeField] private TMP_Text _title;
		[SerializeField] private Image _icon;

		private BoxData _boxData;
		private int _maxOrder;

		private Action<int> _onChoiceAmount;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_okButton.onClick.AddListener(SendToOrder);
			_slider.onValueChanged.AddListener(UpdateOrderAmount);
			_minButton.onClick.AddListener(Min);
			_maxButton.onClick.AddListener(Max);
		}
		#endregion

		#region Public Method
		public void Open(BoxData data, int maxOrder, Action<int> choiceAmount)
		{
			_boxData = data;
			_maxOrder = maxOrder;
			_slider.value = 0f;
			_slider.maxValue = _maxOrder;
			_title.text = data.BoxType.ToString();
			_context.text = _boxData.GetContext();
			_icon.sprite = data.Sprite;
			_onChoiceAmount = choiceAmount;

			UpdateOrderAmount(0);
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
		#endregion

		#region Private Method
		private void SendToOrder()
		{
			int amount = (int)_slider.value;

			if(amount > 0)
			{
				int cost = amount * _boxData.Cost;
				var newOrderData = new BoxOrderData(_boxData.BoxType, amount);
				_boxOrder.AddOrderData(newOrderData, cost);
			}

			_onChoiceAmount?.Invoke(amount);

			Close();
		}

		private void UpdateOrderAmount(float value)
		{
			_slider.value = Mathf.RoundToInt(value);
			int current = (int)_slider.value;
			_orderAmount.text = $"{current} / {_maxOrder}";
		}

		private void Max()
		{
			_slider.value = _maxOrder;
		}

		private void Min()
		{
			_slider.value = 0f;
		}


		#endregion

		#region Protected Method
		#endregion

	}
}
