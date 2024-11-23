using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AI;
using EverythingStore.Optimization;
using EverythingStore.Sell;
using System;
using UnityEngine;
using static EverythingStore.InteractionObject.PickableObject;

namespace EverythingStore.InteractionObject
{
	public class Counter : MonoBehaviour, IPlayerInteraction, ICustomerInteraction, IWaitingInteraction, IEnterPoint, IInteractionPoint, IEnterableCustomer
	{
		#region Field
		[SerializeField] private ObjectPoolManger _poolManger;

		[SerializeField] private SellPackage _prefab;
		[SerializeField] private WaitingLine _watingLine;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _interactionPoint;
		[SerializeField] private MoneySpawner _moneySpawner;
		[SerializeField] private int _maxCustomer;

		private SellPackage _sellpackage;
		private Customer _useCustomer;
		private int _money;
		private bool _isUsedCustomer;
		private int _enterCustomerCount = 0;
		#endregion

		#region Property
		/// <summary>
		/// ���� ��ǰ�� ����
		/// </summary>
		public int Money => _money;

		public bool IsWaitingLineEmpty => _watingLine.CustomerCount == 0;

		public Vector3 EnterPoint => _enterPoint.position;

		public Vector3 InteractionPoint => _interactionPoint.position;
		#endregion

		#region Event
		/// <summary>
		/// �������� ��� �Ϸ�Ǿ��� �� ȣ��˴ϴ�.
		/// </summary>
		public event Action OnFinshCalculation;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_maxCustomer = _watingLine.Max + 1;
		}
		private void Start()
		{
			SpawnPackage();
		}


		#endregion

		#region Public Method
		//�մ� ī���� �տ� �� ��Ȳ
		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (hand.CanDrop() == false)
			{
				return;
			}
			
			var sellObject = hand.Drop(_sellpackage.PackagePoint, Vector3.zero).GetComponent<SellObject>();
			_money += sellObject.Money;
		}

		public void InteractionPlayer(Player player)
		{
			if (_sellpackage == null || _useCustomer == null)
			{
				return;
			}

			//�մ��� �Ⱦ��� ������Ʈ�� �ִ� ���
			if(_useCustomer.pickupAndDrop.HasPickupObject() == true)
			{
				return;
			}

			//������ �Ǿ����� �ʾҴٸ� ������ �Ѵ�.
			if (_sellpackage.IsPackage == false)
			{
				_sellpackage.Package();
			}
			//�մ� �տ� �ƹ��͵� ���� ���
			else
			{
				SendPackageToCustomer();
				CreateMoney(_money);
				_money = 0;
				ExitToCustomer();
			}

		}

		/// <summary>
		/// Test�� �ڵ�
		/// </summary>
		public void Package()
		{
			_sellpackage.Package();
		}

		/// <summary>
		/// �մԿ��� �������� �ǳ��ݴϴ�.
		/// </summary>
		public void SendPackageToCustomer()
		{
			_useCustomer.pickupAndDrop.Pickup(_sellpackage);
		}

		public bool IsEmpty()
		{
			return _useCustomer == null;
		}
		
		public bool IsWaitingEmpty()
		{
			return _useCustomer == null && _watingLine.CustomerCount == 0;
		}

		public FSMStateType EnterInteraction(Customer customer)
		{
			_useCustomer = customer;
			_isUsedCustomer = true;
			RemoveEnterMoveCustomer();
			return FSMStateType.Customer_MoveTo_Counter;
		}

		public FSMStateType EnterWaitingLine(Customer customer)
		{
			_watingLine.EnqueueCustomer(customer);
			RemoveEnterMoveCustomer();
			return FSMStateType.Stop;
		}
		#endregion

		#region Private Method

		/// <summary>
		/// ���� �����մϴ�.
		/// </summary>
		private void CreateMoney(int money)
		{
			_moneySpawner.AddMoney(money);
		}

		/// <summary>
		/// �մԿ��� ������� ��û�մϴ�.
		/// </summary>
		private void ExitToCustomer()
		{
			Debug.Log("�մ� ������� ��û");
			_useCustomer = null;
			SpawnPackage();
			_isUsedCustomer = false;

			if (_watingLine.CustomerCount > 0)
			{
			_useCustomer = _watingLine.DequeueCustomer();
				_isUsedCustomer = true;
				_useCustomer.MoveToCounter();
			}
		}

		/// <summary>
		/// ��Ű���� �����մϴ�.
		/// </summary>
		private void SpawnPackage()
		{
			_sellpackage = _poolManger.GetPoolObject(PooledObjectType.Package).GetComponent<SellPackage>();
			_sellpackage.transform.parent = _spawnPoint;
			_sellpackage.transform.localPosition = Vector3.zero;
		}

		public bool IsEnterable()
		{
			int totalCustomer = _enterCustomerCount + _watingLine.CustomerCount;
			if(_isUsedCustomer == true)
			{
				totalCustomer++;
			}
			return _maxCustomer > totalCustomer;
		}

		public void AddEnterMoveCustomer()
		{
			_enterCustomerCount++;
		}

		public void RemoveEnterMoveCustomer()
		{
			_enterCustomerCount--;
		}


		#endregion
	}
}

