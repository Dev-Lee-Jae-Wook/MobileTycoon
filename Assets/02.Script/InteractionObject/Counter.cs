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
		/// 구매 상품의 총합
		/// </summary>
		public int Money => _money;

		public bool IsWaitingLineEmpty => _watingLine.CustomerCount == 0;

		public Vector3 EnterPoint => _enterPoint.position;

		public Vector3 InteractionPoint => _interactionPoint.position;
		#endregion

		#region Event
		/// <summary>
		/// 계산과정이 모두 완료되었을 때 호출됩니다.
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
		//손님 카운터 앞에 선 상황
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

			//손님이 픽업한 오브젝트가 있는 경우
			if(_useCustomer.pickupAndDrop.HasPickupObject() == true)
			{
				return;
			}

			//포장이 되어있지 않았다면 포장을 한다.
			if (_sellpackage.IsPackage == false)
			{
				_sellpackage.Package();
			}
			//손님 손에 아무것도 없는 경우
			else
			{
				SendPackageToCustomer();
				CreateMoney(_money);
				_money = 0;
				ExitToCustomer();
			}

		}

		/// <summary>
		/// Test용 코드
		/// </summary>
		public void Package()
		{
			_sellpackage.Package();
		}

		/// <summary>
		/// 손님에게 포장지를 건네줍니다.
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
		/// 돈을 생성합니다.
		/// </summary>
		private void CreateMoney(int money)
		{
			_moneySpawner.AddMoney(money);
		}

		/// <summary>
		/// 손님에게 나가라고 요청합니다.
		/// </summary>
		private void ExitToCustomer()
		{
			Debug.Log("손님 나가라고 요청");
			_useCustomer = null;
			SpawnPackage();

			if(_watingLine.CustomerCount > 0)
			{
			_useCustomer = _watingLine.DequeueCustomer();
				_useCustomer.MoveToCounter();
			}
		}

		/// <summary>
		/// 패키지를 생성합니다.
		/// </summary>
		private void SpawnPackage()
		{
			_sellpackage = Instantiate(_prefab, _spawnPoint);
		}


		#endregion
	}
}
