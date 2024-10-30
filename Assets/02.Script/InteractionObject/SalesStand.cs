using EverythingStore.Actor;
using System.Collections.Generic;
using UnityEngine;

//��í Ȯ�� �����ʹ� �� �� ������ �ʿ��մϴ�.
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
		//����׿� ���� ��ġ�� ǥ���մϴ�.
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
		/// �ǸŴ��� Top�� ��ġ�� �������� ��ȯ�մϴ�.
		/// </summary>
		private SellObject PopSellObject()
		{
			return _salesObjectStack.Pop();
		}

		/// <summary>
		/// �ǸŴ뿡�� �������� �߰� �մϴ�.
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
		/// �Ǹ� ��ǰ�� �ǸŴ뿡 ���� �� �ִ����� Ȯ���մϴ�.
		/// </summary>
		private bool CanPushSellObject()
		{
			return _salesObjectStack.Count < _capacity;
		}

		/// <summary>
		/// �ǸŴ뿡�� ��ǰ�� �� �� �ִ��� Ȯ���մϴ�.
		/// </summary>
		private bool CanPopSellObject()
		{
			return _salesObjectStack.Count > 0;
		}
		#endregion

		#endregion
	}
}