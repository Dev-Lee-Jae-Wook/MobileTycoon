using EverythingStore.Actor;
using System;
using System.Collections;
using System.Collections.Generic;
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
			//손에 구매 상품이 없다면
			if(hand.IsPickUpObject() == false)
			{
				return;
			}

			if(hand.PeekObject().type != PickableObjectType.SellObject)
			{
				return;
			}

			_customer = hand;
			_sellpackage.AddSellItem(hand.Pop().GetComponent<SellObject>());
			
		}

		public void InteractionPlayer(PickupAndDrop hand)
		{
			if(_sellpackage == null && _sellpackage.IsPackage == false)
			{
				return;
			}

			if(_customer.pickUpObjectCount > 0)
			{
				return;
			}

			int money = _sellpackage.Package();
			SendPackageToCustomer();
			CreateMoney(money);
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
