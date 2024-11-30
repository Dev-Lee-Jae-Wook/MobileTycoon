using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AI;
using EverythingStore.AssetData;
using EverythingStore.Sell;
using EverythingStore.Upgrad;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//��í Ȯ�� �����ʹ� �� �� ������ �ʿ��մϴ�.
namespace EverythingStore.InteractionObject
{
	public class SalesStand : MonoBehaviour, IPlayerInteraction, ICustomerInteraction, IInteractionPoint, IWaitingLine, IEnterPoint, IEnterableCustomer, IUpgradInt
	{
		#region Field
		private Stack<SellObject> _salesObjectStack = new Stack<SellObject>();
		[SerializeField] private int _capacity;
		[SerializeField] private Transform _interactionPoint;
		[SerializeField] private WaitingLine _waitingLine;
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private GameObject[] _models;
		[SerializeField] private int _maxCustomer;

		[Title("Upgrad")]
		[SerializeField] private SaleStandPivotData[] _pivotDatas;
		private int _upgradLv = 0;

		[Title("Pivot")]
		[SerializeField] private Transform _pivot;
		[SerializeField] private SaleStandPivotData _pivotData;

		private Customer _useCustomer;
		private GameObject _currentMode;
		private bool _isCustomerInteraction = true;
		private int _enterMoveCustomerCount = 0;
		private bool _isUsedCustomer = false;
		private int _maxCapacity = 18;

		private InputMoneyArea _upgradArea;
		#endregion

		#region Property
		public Transform Pivot => _pivot;
		public SaleStandPivotData PivotData => _pivotData;
		public int Capacity => _capacity;

		public Vector3 InteractionPoint => _interactionPoint.position;

		public Vector3 EnterPoint => _enterPoint.position;

		public int MaxCustomer => _waitingLine.Max + 1;
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
				Vector3 worldPos =  transform.position + PivotData.PivotLocalPos + p;
				Gizmos.DrawCube(worldPos, Vector3.one * 0.1f);
			}
		}

		private void Awake()
		{
			_upgradArea = transform.GetComponentInChildren<InputMoneyArea>();
			_currentMode = _models[0];
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
				_isCustomerInteraction = false;
				var sellObject = pickupAndDrop.PeekObject().GetComponent<SellObject>();
				pickupAndDrop.Drop(Pivot, GetCurrentSloatPosition(), (pickupObject) =>
				{
					pickupObject.transform.localRotation = Quaternion.Euler(0.0f, 180f, 0.0f);
					PushSellObject(sellObject);

					if (pickupAndDrop.IsRunningDrop() == false)
					{
						_isCustomerInteraction = true;
					}
				});
			}
		}

		/// <summary>
		/// �մ��� �Ǹ�ǰ�� �Ⱦ��ϰ� �����մϴ�.
		/// </summary>
		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (_isCustomerInteraction == false)
			{
				return;
			}

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
		}
		
		/// <summary>
		/// �ǸŴ��� Top�� ��ġ�� �������� ��ȯ�մϴ�.
		/// </summary>
		private SellObject PopSellObject()
		{
			var sellObject = _salesObjectStack.Pop();
			sellObject.transform.localScale = Vector3.one;
			return sellObject;
		}

		/// <summary>
		/// �ǸŴ뿡�� �������� �߰� �մϴ�.
		/// </summary>
		/// <param name="sellObject"></param>
		public void PushSellObject(SellObject sellObject)
		{
			sellObject.transform.localScale = Vector3.one * 0.6f;
			_salesObjectStack.Push(sellObject);
		}

		public bool IsWaitingEmpty()
		{
			return _useCustomer == null && _waitingLine.CustomerCount == 0;
		}

		public FSMStateType EnterInteraction(Customer customer)
		{
			_useCustomer = customer;
			_isUsedCustomer = true;
			RemoveEnterMoveCustomer();
			return FSMStateType.Customer_MoveTo_SalesStation;
		}

		public FSMStateType EnterWaitingLine(Customer customer)
		{
			_waitingLine.EnqueueCustomer(customer);
			RemoveEnterMoveCustomer();
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

		public void ExitCustomer()
		{
			_useCustomer = null;
			_isUsedCustomer = false;

			if (_waitingLine.CustomerCount > 0)
			{
				_useCustomer = _waitingLine.DequeueCustomer();
				_isUsedCustomer = true;
				_useCustomer.MoveToSaleStation();
			}
		}

		public void Upgrad(int upgradCount, int capacity, SaleStandPivotData pivotData)
		{
			_capacity = capacity;
			_currentMode.SetActive(false);
			_currentMode = _models[upgradCount + 1];
			_currentMode.SetActive(true);
			_pivotData = pivotData;
			Pivot.localPosition = pivotData.PivotLocalPos;
			UpdateSellItem();
		}

		private void UpdateSellItem()
		{
			int index = 0;

			foreach(var item in _salesObjectStack.Reverse())
			{
				item.transform.localPosition = PivotData.PivotPoints[index++];
			}
		}

		public bool IsEnterable()
		{
			//��Ȱ��ȭ �ÿ��� ������ �������� ��ȯ
			if (gameObject.activeSelf == false) 
				return false;

			int totalCustomer = _waitingLine.CustomerCount + _enterMoveCustomerCount;
			if(_isUsedCustomer == true)
			{
				totalCustomer++;
			}

			return _maxCustomer > totalCustomer;
		}

		public void AddEnterMoveCustomer()
		{
			_enterMoveCustomerCount++;
		}

		public void RemoveEnterMoveCustomer()
		{
			_enterMoveCustomerCount--;
		}

		public void Upgrad(int value)
		{
			_capacity = value;
			_pivotData = _pivotDatas[_upgradLv];
			_currentMode.SetActive(false);
			_currentMode = _models[_upgradLv + 1];
			_currentMode.SetActive(true);
			Pivot.localPosition = _pivotData.PivotLocalPos;

			UpdateSellItem();
			_upgradLv++;
		}

		internal void MaxUpgrad()
		{
			_capacity = _maxCapacity;
			_pivotData = _pivotDatas[_pivotDatas.Length - 1];
			_currentMode.SetActive(false);
			_currentMode = _models.Last();
			_currentMode.SetActive(true);
			Pivot.localPosition = _pivotData.PivotLocalPos;
			_upgradArea.gameObject.SetActive(false);
			UpdateSellItem();
		}
		#endregion
	}
}