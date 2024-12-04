using EverythingStore.Actor.Player;
using EverythingStore.Delivery;
using EverythingStore.GameEvent;
using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.BoxBox
{
	public class BoxOrder : PopupUIBase
	{
		#region Field
		[Title("Collaboration")]
		[SerializeField] private Player _player;
		[SerializeField] private DeliveryManger _deliveryManger;
		[SerializeField] private BoxStorage _boxStorage;
		[SerializeField] private ChoiceBox _choiceBox;

		[Title("Button")]
		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _resetButton;
		[SerializeField] private Transform _boxOrderItemParent;

		[Title("Text")]
		[SerializeField] private TMP_Text _totalOrder;
		[SerializeField] private TMP_Text _totalCost;
		[SerializeField] private TMP_Text _boxStorageText;

		private Wallet _playerWallet;

		private BoxOrderItem[] _orderBoxItems;

		private List<BoxOrderData> _orderBoxDataList = new();
		private int _useMoney;

		private int _maxOrder;
		#endregion

		#region Property
		#endregion

		#region Event
		public event Action OnOrderDelivery;
		#endregion


		#region Public Method
		private void Init()
		{
			ResetOrder();
			_maxOrder = _boxStorage.FreeSpace;
			_boxStorageText.text = $"Box Stroage {_boxStorage.Count} / {_boxStorage.Capacity}";
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
		/// <summary>
		/// 구매하고자하는 박스를 구매하면 택배가 배달해줌
		/// </summary>
		private void Buy()
		{
			var orderDatas = _orderBoxDataList.ToArray();

			if (orderDatas.Length > 0)
			{
				_deliveryManger.SetOrderData(orderDatas);
				_playerWallet.SubtractMoney(_useMoney);
				_deliveryManger.StartDelivery();
				OnOrderDelivery?.Invoke();
			}

			PopDown();
		}

		private void PopUpChoiceBox(BoxOrderItem boxOrderItem)
		{
			int orderableCount = GetUseAbleMoney() / boxOrderItem.BoxData.Cost;
			int max = Mathf.Clamp(orderableCount, 0, GetOrderAbleCount());

			_choiceBox.Open(boxOrderItem.BoxData, max, boxOrderItem.UpDateAmount);
		}

		private void ResetOrder()
		{
			_orderBoxDataList.Clear();
			_useMoney = 0;
			UpdateBoxOrderItem();
			UpdateOrderInfo();

			foreach (var item in _orderBoxItems)
			{
				item.UpDateAmount(0);
			}
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
			foreach (var item in _orderBoxItems)
			{
				bool isBlock = item.Cost > useableMoney;
				item.SetBlock(isBlock);
			}
		}

		private void UpdateOrderInfo()
		{
			int totalOrder = 0;
			int totalCost = _useMoney;

			foreach (var item in _orderBoxDataList)
			{
				totalOrder += item.Amount;
			}

			_totalOrder.text = $"Total Order : {totalOrder}";
			_totalCost.text = $"Total Cost : {totalCost}";
		}
		#endregion

		#region Protected Method
		protected override void StartInit()
		{
			OnOpen += Init;

			_resetButton.onClick.AddListener(ResetOrder);
			_buyButton.onClick.AddListener(Buy);

			_playerWallet = _player.Wallet;
			_orderBoxItems = _boxOrderItemParent.GetComponentsInChildren<BoxOrderItem>();


			foreach (var boxItem in _orderBoxItems)
			{
				boxItem.Init(() => PopUpChoiceBox(boxItem));
			}

			_choiceBox.Close();
		}
		#endregion
	}
}