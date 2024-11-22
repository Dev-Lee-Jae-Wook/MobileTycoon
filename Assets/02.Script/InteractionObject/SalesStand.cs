using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AI;
using EverythingStore.AssetData;
using EverythingStore.Sell;
using System;
using System.Collections.Generic;
using UnityEngine;

//��í Ȯ�� �����ʹ� �� �� ������ �ʿ��մϴ�.
namespace EverythingStore.InteractionObject
{
	public class SalesStand : MonoBehaviour, IPlayerInteraction, ICustomerInteraction, IInteractionPoint, IWaitingInteraction, IEnterPoint
	{
		#region Field
		private Stack<SellObject> _salesObjectStack = new Stack<SellObject>();
		[SerializeField] private int _capacity;
		[SerializeField] private Transform _interactionPoint;
		[SerializeField] private WaitingLine _waitingLine;
		[SerializeField] private Transform _enterPoint;
		private Customer _useCustomer;
		#endregion

		#region Property
		[field:SerializeField] public Transform Pivot { get; private set; }
		[field: SerializeField] public SaleStandPivotData PivotData { get; private set; }
		public int Capacity => _capacity;

		public Vector3 InteractionPoint => _interactionPoint.position;

		public Vector3 EnterPoint => _enterPoint.position;
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

		#region Public Method 

		/// <summary>
		/// �÷��̾� �տ� �ִ� �Ǹ�ǰ�� �ǸŴ뿡 �����մϴ�.
		/// </summary>
		public void InteractionPlayer(Player player)
		{
			var pickupAndDrop = player.PickupAndDrop;

			if (pickupAndDrop.HasPickupObject() == false || pickupAndDrop.CanDrop() == false)
			{
				return;
			}

			if (CanPushSellObject() == false)
			{
				return;
			}


			if (pickupAndDrop.PeekObject().type == PickableObjectType.SellObject)
			{
				var sellObject = pickupAndDrop.PeekObject().GetComponent<SellObject>();
				pickupAndDrop.Drop(Pivot, GetCurrentSloatPosition(), () =>
				{
					sellObject.transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
					PushSellObject(sellObject);
				});
			}
		}

		/// <summary>
		/// �մ��� �Ǹ�ǰ�� �Ⱦ��ϰ� �����մϴ�.
		/// </summary>
		public void InteractionCustomer(PickupAndDrop hand)
		{
			if(_useCustomer.pickupAndDrop != hand)
			{
				return;
			}

			if (hand.CanPickup(PickableObjectType.SellObject) == false)
			{
				return;
			}

			if (CanPopSellObject() == false)
			{
				return;
			}

			hand.Pickup(PopSellObject());
			ExitCustomer();
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

		public bool IsWaitingEmpty()
		{
			return _useCustomer == null && _waitingLine.CustomerCount == 0;
		}

		public FSMStateType EnterInteraction(Customer customer)
		{
			_useCustomer = customer;
			return FSMStateType.Customer_MoveTo_SalesStation;
		}

		public FSMStateType EnterWaitingLine(Customer customer)
		{
			_waitingLine.EnqueueCustomer(customer);
			return FSMStateType.Stop;
		}

		#endregion

		#region Private Method
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

		internal Vector3 GetInteractionPoint()
		{
			return _interactionPoint.position;
		}

		private void ExitCustomer()
		{
			_useCustomer = null;

			if (_waitingLine.CustomerCount > 0)
			{
				_useCustomer = _waitingLine.DequeueCustomer();
				_useCustomer.MoveToSaleStation();
			}
		}
		#endregion
	}
}