using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Sell;
using System;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class AuctionItem : MonoBehaviour, ICustomerInteraction
	{
		#region Field
		[SerializeField] private Transform _itemPoint;
		private SellObject _sellObject = null;
		#endregion

		#region Property
		public Transform ItemPoint => _itemPoint;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void InteractionCustomer(PickupAndDrop hand)
		{
		}

		/// <summary>
		/// ��� ��ǰ�� ��ǰ�� �ִ���?
		/// </summary>
		/// <returns></returns>
		internal bool HasItem()
		{
			return _sellObject != null;
		}

		/// <summary>
		/// ��� ��ǰ�� �����Ѵ�.
		/// </summary>
		internal void Setup(SellObject item)
		{
			_sellObject = item;
		}
		#endregion

		#region Private Method
		#endregion

		#region Protected Method
		#endregion
	}
}
