using EverythingStore.Actor.Player;
using EverythingStore.Delivery;
using EverythingStore.InteractionObject;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.BoxBox
{
	public class BoxOrder : MonoBehaviour
	{
		#region Field
		[SerializeField]private Player _player;
		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _resetButton;
		[SerializeField] private Button _closeButton;
		[SerializeField] private DeliveryManger _deliveryManger;
		[SerializeField] private BoxStorage _boxStorage;
		[SerializeField] private Transform _boxOrderItemParent;
		[SerializeField] private ChoiceBox _choiceBox;

		[SerializeField] private TMP_Text _totalOrder;
		[SerializeField] private TMP_Text _totalCost;


		private Canvas _canvas;
		private Wallet _playerWallet;

		private BoxOrderItem[] _orderBoxItems;

		private List<BoxOrderData> _orderBoxDataList = new();
		private int _useMoney;

		private int _maxOrder;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle

		private void Start()
		{
			_resetButton.onClick.AddListener(ResetOrder);
			_buyButton.onClick.AddListener(Buy);
			_closeButton.onClick.AddListener(Close);

			_playerWallet = _player.Wallet;
			_canvas = transform.parent.GetComponent<Canvas>();
			_orderBoxItems = _boxOrderItemParent.GetComponentsInChildren<BoxOrderItem>();


			foreach (var boxItem in _orderBoxItems)
			{
				boxItem.Init(() => PopUpChoiceBox(boxItem.BoxData));
			}

			Toggle(false);
			_choiceBox.gameObject.SetActive(false);
		}
		#endregion

		#region Public Method
		public void Open()
		{
			Toggle(true);
			_useMoney = 0;
			_maxOrder = _boxStorage.FreeSpace;
			UpdateBoxOrderItem();
			UpdateOrderInfo();
		}

		public void AddOrderData(BoxOrderData newOrderData, int cost)
		{
			_useMoney += cost;
			_orderBoxDataList.Add(newOrderData);
			UpdateBoxOrderItem();
			UpdateOrderInfo();
		}
		#endregion

		#region Private Method
		private void Buy()
		{
			var copyOrderData = _orderBoxDataList.ToArray();
			_deliveryManger.AddOrderData(copyOrderData);
			_playerWallet.SubtractMoney(_useMoney);

			ResetOrder();
			Close();
		}

		private void Close()
		{
			Toggle(false);
		}

		private void Toggle(bool isToggle)
		{
			_canvas.enabled = isToggle;
		}

		private void PopUpChoiceBox(BoxData data)
		{
			int orderableCount = GetUseAbleMoney() / data.Cost;
			int max = Mathf.Clamp(orderableCount, 0, GetOrderAbleCount());

			_choiceBox.Open(data, max);
		}

		private void ResetOrder()
		{
			_orderBoxDataList.Clear();
			_useMoney = 0;
			UpdateBoxOrderItem();
			UpdateOrderInfo();
		}

		private int GetUseAbleMoney()
		{
			return _playerWallet.Money - _useMoney;
		}

		private int GetOrderAbleCount()
		{
			int result = _maxOrder;
			foreach (var item in _orderBoxDataList)
			{
				result -= item.Amount;
			}

			return result;
		}

		private void UpdateBoxOrderItem()
		{
			int useableMoney = GetUseAbleMoney();
			foreach(var item in _orderBoxItems)
			{
				bool isBlock = item.Cost > useableMoney;
				item.SetBlock(isBlock);
			}
		}

		private void UpdateOrderInfo()
		{
			int totalOrder = 0;
			int totalCost = _useMoney;

			foreach(var item in _orderBoxDataList)
			{
				totalOrder += item.Amount;
			}

			_totalOrder.text = $"Total Order : {totalOrder}";
			_totalCost.text = $"Total Cost : {totalCost}";
		}
		#endregion

		#region Protected Method
		#endregion



	}
}