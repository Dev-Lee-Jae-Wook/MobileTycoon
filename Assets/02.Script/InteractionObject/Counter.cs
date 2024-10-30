using EverythingStore.Actor;
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
