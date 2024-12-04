using EverythingStore.BoxBox;
using EverythingStore.GameEvent;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Delivery
{
	public class DeliveryManger : MonoBehaviour
	{
		#region Field
		[SerializeField] private DeliveryTruck _truck;
		private Queue<BoxOrderData[]> _order = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_truck.OnFinshDelivey += () => Debug.Log("배달 완료를 받음");
		}
		#endregion

		#region Public Method
		public void SetOrderData(BoxOrderData[] orderData)
		{
			if (orderData.Length == 0)
			{
				return;
			}

			_truck.SpawnBox(orderData);
		}

		public void StartDelivery()
		{
			if (Tutorial.Instance.isDeliveryBox == false)
			{
				Tutorial.Instance.DeliveryBox();
			}
			else
			{
				_truck.StartDeliveryProcess();
			}
		}
		#endregion

		#region Private Method

		#endregion


	}
}
