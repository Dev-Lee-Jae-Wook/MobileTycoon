using EverythingStore.OrderBox;
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
		public void AddOrderData(BoxOrderData[] orderData)
		{
			if (orderData.Length == 0)
			{
				return;
			}

			if (_truck.IsDelivery == true)
			{
				_order.Enqueue(orderData);
			}
            else
            {
				StartDeilvery(orderData);
            }
        }
		#endregion

		#region Private Method
		private void StartDeilvery(BoxOrderData[] orderData)
		{
			_truck.StartDeliveryProcess(orderData);
		}
		#endregion


	}
}
