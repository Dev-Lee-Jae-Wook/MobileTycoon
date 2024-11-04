using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using System.Collections.Generic;
using UnityEngine;

//��í Ȯ�� �����ʹ� �� �� ������ �ʿ��մϴ�.
namespace EverythingStore.InteractionObject
{
	public class SalesStand : MonoBehaviour, IPlayerInteraction, ICustomerInteraction
	{
		#region Field
		private Stack<SellObject> _salesObjectStack = new Stack<SellObject>();
		[SerializeField] private int _capacity;
		#endregion

		#region Property
		[field:SerializeField] public Transform Pivot { get; private set; }
		[field: SerializeField] public SaleStandPivotData PivotData { get; private set; }
		public int Capacity => _capacity;
		#endregion




		#region UnityCycle
		//����׿� ���� ��ġ�� ǥ���մϴ�.
		private void OnDrawGizmos()
		{
			if(PivotData == null)
			{
				return;
			}

			Gizmos.color = Color.cyan;
			foreach (Vector3 p in PivotData.PivotPoints)
			{
				Vector3 worldPos = Pivot.position + p;
				Gizmos.DrawCube(worldPos, Vector3.one * 0.1f);
			}
		}
		#endregion

		#region Method

		#region Public
		public void InteractionPlayer(PickupAndDrop hand)
		{
			if (hand.HasPickupObject() == false || hand.CanPopup() == false)
			{
				return;
			}

			if (CanPushSellObject() == false)
			{
				return;
			}


			if (hand.PeekObject().type == PickableObject.PickableObjectType.SellObject)
			{
				var sellObject = hand.PeekObject().GetComponent<SellObject>();
				hand.ParabolaDrop(Pivot, GetCurrentSloatPosition(), () =>
				{
					sellObject.transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
					PushSellObject(sellObject);
				});
			}
		}

		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (hand.CanPickup() == false)
			{
				return;
			}

			if (CanPopSellObject() == false)
			{
				return;
			}

			hand.ProductionPickup(PopSellObject());
		}
		
		/// <summary>
		/// �ǸŴ��� Top�� ��ġ�� �������� ��ȯ�մϴ�.
		/// </summary>
		public SellObject PopSellObject()
		{
			return _salesObjectStack.Pop();
		}

		/// <summary>
		/// �ǸŴ뿡�� �������� �߰� �մϴ�.
		/// </summary>
		/// <param name="sellObject"></param>
		public void PushSellObject(SellObject sellObject)
		{
			_salesObjectStack.Push(sellObject);
		}
		#endregion

		#region Private


		private Vector3 GetCurrentSloatPosition()
		{
			return PivotData.PivotPoints[_salesObjectStack.Count];
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