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
		/// <summary>
		/// ���� ��ǰ�� ����
		/// </summary>
		public int Money => _money;
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
			if (hand.CanPopup() == false)
			{
				return;
			}

			if (hand.PeekObject().type != PickableObjectType.SellObject)
			{
				return;
			}

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
			else if (_customer.CanPickup() == true)
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
			_customer.ProductionPickup(_sellpackage);
		}
		#endregion

		#region Private Method


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
		public void ExitToCustomer()
		{
			Debug.Log("�մ� ������� ��û");
			_customer = null;
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
