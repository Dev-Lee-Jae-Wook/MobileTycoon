using EverythingStore.OrderBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Delivery
{
	public class DeliveryManger : MonoBehaviour
	{
		#region Field
		private Queue<BoxOrderData[]> _order = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
		internal void AddOrderData(BoxOrderData[] orderData)
		{
			if(orderData.Length == 0)
			{
				return;
			}

			foreach (var item in orderData)
			{
				Debug.Log($"{item.Type} {item.Amount}");
			}
		}
	}
}
