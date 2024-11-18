using EverythingStore.Actor.Player;
using EverythingStore.Delivery;
using EverythingStore.InteractionObject;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EverythingStore.BoxBox
{
	public class BoxOrder : MonoBehaviour
	{
		#region Field
		[SerializeField]private Player _player;
		[SerializeField] private Button _closeButton;
		[SerializeField] private DeliveryManger _deliveryManger;
		[SerializeField] private Transform _boxOrderItemParent;
		private Canvas _canvas;
		private List<BoxOrderData> _orderList = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_canvas = transform.parent.GetComponent<Canvas>();
			_closeButton.onClick.AddListener(SendOrderData);
		}

		private void Start()
		{
			var boxOrderItems =_boxOrderItemParent.GetComponentsInChildren<BoxOrderItem>();
			foreach (var item in boxOrderItems)
			{
				item.Init(_player.Wallet);
			}
			Toggle(false);
		}
		#endregion

		#region Public Method
		public void Open()
		{
			Toggle(true);
		}
		public void AddOrderData(BoxOrderData data)
		{
			_orderList.Add(data);
		}

		#endregion

		#region Private Method
		private void SendOrderData()
		{
			var copyOrderData = _orderList.ToArray();
			_deliveryManger.AddOrderData(copyOrderData);
			_orderList.Clear();
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
		#endregion

		#region Protected Method
		#endregion



	}
}