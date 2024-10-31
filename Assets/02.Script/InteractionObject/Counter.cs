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
		private SellPackage _sellpackage;
		private PickupAndDrop _customer;
		private int _money;
		#endregion

		#region Property
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

			_customer = hand;
			var sellObject = hand.Pop(_sellpackage.PackagePoint, Vector3.zero).GetComponent<SellObject>();
			_sellpackage.AddSellItem(sellObject);

		}

		public void InteractionPlayer(PickupAndDrop hand)
		{
			if (_sellpackage == null || _customer == null)
			{
				return;
			}

			if (_sellpackage.IsPackage == false)
			{
				_money = _sellpackage.Package();
			}
			else if (_customer.CanPickup() == true)
			{
				SendPackageToCustomer();
				CreateMoney(_money);
				_money = 0;
				ExitToCustomer();
			}

		}
		#endregion

		#region Private Method

		/// <summary>
		/// 손님에게 포장지를 건네줍니다.
		/// </summary>
		private void SendPackageToCustomer()
		{
			_customer.PickUp(_sellpackage);
		}

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
		private void ExitToCustomer()
		{
			Debug.Log("손님 나가라고 요청");
			_customer = null;
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
