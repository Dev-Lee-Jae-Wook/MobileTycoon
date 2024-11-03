using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using System;
using UnityEngine;
using static EverythingStore.InteractionObject.PickableObject;

namespace EverythingStore.InteractionObject
{
	public class Counter : MonoBehaviour, IPlayerInteraction, ICustomerInteraction
	{
		#region Field
		[SerializeField] private SellPackage _prefab;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private WatingLine _watingLine;
		private SellPackage _sellpackage;
		private PickupAndDrop _customer;
		private int _money;
		#endregion

		#region Property
		/// <summary>
		/// 구매 상품의 총합
		/// </summary>
		public int Money => _money;

		public bool IsWaitingLineEmpty => _watingLine.WaitingCustomerCount == 0;
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

			if (hand.PeekObject().type != PickableObjectType.SellObject)
			{
				return;
			}
			//손님이 상품을 놓도록한다.
			_customer = hand;
			var sellObject = hand.ProductionDrop(_sellpackage.PackagePoint, Vector3.zero).GetComponent<SellObject>();
			_money += sellObject.Money;
		}

		public void InteractionPlayer(PickupAndDrop hand)
		{
			if (_sellpackage == null || _customer == null)
			{
				return;
			}

			if (_sellpackage.IsPackage == false)
			{
				_sellpackage.Package();
			}
			//손님 손에 아무것도 없는 경우
			else if (_customer.CanPickup() == true)
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
			_customer.ProductionPickup(_sellpackage);
		}

		public bool IsEmpty()
		{
			return _customer == null;
		}

		public void EnterWaitingLine(Customer customer)
		{
			_watingLine.EnqueueCustomer(customer);
		}
		#endregion

		#region Private Method

		/// <summary>
		/// 돈을 생성합니다.
		/// </summary>
		private void CreateMoney(int money)
		{
			Debug.Log($"돈이 추가됩니다. {money}");
		}

		/// <summary>
		/// 손님에게 나가라고 요청합니다.
		/// </summary>
		public void ExitToCustomer()
		{
			Debug.Log("손님 나가라고 요청");
			_customer = null;
			SpawnPackage();

			//여기 작업해야됨
			if (_watingLine.WaitingCustomerCount > 0)
			{
				_watingLine.DequeueCustomer().OnTriggerGoToCounter();
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
