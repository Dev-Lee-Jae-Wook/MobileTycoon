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
		public void InteractionCustomer(PickupAndDrop hand)
		{
			//�տ� ���� ��ǰ�� ���ٸ�
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
		/// �մԿ��� �������� �ǳ��ݴϴ�.
		/// </summary>
		private void SendPackageToCustomer()
		{
			_customer.PickUp(_sellpackage);
		}

		/// <summary>
		/// ���� �����մϴ�.
		/// </summary>
		private void CreateMoney(int money)
		{
			Debug.Log($"���� �߰��˴ϴ�. {money}");
		}

		/// <summary>
		/// �մԿ��� ������� ��û�մϴ�.
		/// </summary>
		private void ExitToCustomer()
		{
			Debug.Log("�մ� ������� ��û");
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
