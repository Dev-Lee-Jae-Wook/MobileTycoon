using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AI;
using System;
using UnityEngine;
using static EverythingStore.InteractionObject.PickableObject;

namespace EverythingStore.InteractionObject
{
	public class Counter : MonoBehaviour, IPlayerInteraction, ICustomerInteraction, IWaitingInteraction, IEnterPoint, IInteractionPoint
	{
		#region Field
		[SerializeField] private SellPackage _prefab;
		[SerializeField] private WaitingLine _watingLine;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _interactionPoint;
		[SerializeField] private MoneySpawner _moneySpawner;

		private SellPackage _sellpackage;
		private Customer _useCustomer;
		private int _money;
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
		private void Start()
		{
			SpawnPackage();
		}


		#endregion

		#region Public Method
		//�մ� ī���� �տ� �� ��Ȳ
		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (hand.CanPopup() == false)
			{
				return;
			}
			
			var sellObject = hand.ParabolaDrop(_sellpackage.PackagePoint, Vector3.zero).GetComponent<SellObject>();
			_money += sellObject.Money;
		}

		public void InteractionPlayer(PickupAndDrop hand)
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
			_useCustomer.pickupAndDrop.ProductionPickup(_sellpackage);
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
			return FSMStateType.Customer_MoveTo_Counter;
		}

		public FSMStateType EnterWaitingLine(Customer customer)
		{
			_watingLine.EnqueueCustomer(customer);
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

			if(_watingLine.CustomerCount > 0)
			{
			_useCustomer = _watingLine.DequeueCustomer();
				_useCustomer.MoveToCounter();
			}
		}

		/// <summary>
		/// ��Ű���� �����մϴ�.
		/// </summary>
		private void SpawnPackage()
		{
			_sellpackage = Instantiate(_prefab, _spawnPoint);
		}


		#endregion
	}
}
