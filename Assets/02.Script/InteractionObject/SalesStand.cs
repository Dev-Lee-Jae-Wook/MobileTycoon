using EverythingStore.Actor;
using System.Collections.Generic;
using UnityEngine;

//가챠 확률 데이터는 좀 더 개량이 필요합니다.
namespace EverythingStore.InteractionObject
{
	public class SalesStand : MonoBehaviour, IPlayerInteraction, ICustomerInteraction
	{
		#region Field
		private Stack<SellObject> _salesObjectStack = new Stack<SellObject>();
		[SerializeField] private Transform _pivot;
		[SerializeField] private SaleStandPivotData _pivotData;
		[SerializeField] private int _capacity;

		public int Capacity => _capacity;
		#endregion


		#region UnityCycle
		//디버그용 놓일 위치를 표시합니다.
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			foreach (Vector3 p in _pivotData.PivotPoints)
			{
				Gizmos.DrawCube(p, Vector3.one * 0.1f);
			}
		}
		#endregion

		#region Method

		#region Public
		public void InteractionPlayer(PickupAndDrop hand)
		{
			if (hand.IsPickUpObject() == false)
			{
				return;
			}

			if (CanPushSellObject() == false)
			{
				return;
			}


			if (hand.PeekObject().type == PickableObject.PickableObjectType.SellObject)
			{
				PushSellObject(hand.Pop().GetComponent<SellObject>());
			}
		}

		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (hand.IsPickUpObject() == true)
			{
				return;
			}

			if (CanPopSellObject() == false)
			{
				return;
			}

			hand.PickUp(PopSellObject());
		}
		#endregion

		#region Private
		/// <summary>
		/// 판매대의 Top에 위치한 아이템을 반환합니다.
		/// </summary>
		private SellObject PopSellObject()
		{
			return _salesObjectStack.Pop();
		}

		/// <summary>
		/// 판매대에서 아이템을 추가 합니다.
		/// </summary>
		/// <param name="sellObject"></param>
		private void PushSellObject(SellObject sellObject)
		{
			_salesObjectStack.Push(sellObject);

			sellObject.transform.parent = _pivot;
			sellObject.transform.position = _pivotData.PivotPoints[_salesObjectStack.Count - 1];
			sellObject.transform.localRotation = Quaternion.identity;
		}

		/// <summary>
		/// 판매 상품을 판매대에 넣을 수 있는지를 확인합니다.
		/// </summary>
		private bool CanPushSellObject()
		{
			return _salesObjectStack.Count < _capacity;
		}

		/// <summary>
		/// 판매대에서 상품을 뺄 수 있는지 확인합니다.
		/// </summary>
		private bool CanPopSellObject()
		{
			return _salesObjectStack.Count > 0;
		}
		#endregion

		#endregion
	}
}