using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AuctionSystem;
using EverythingStore.Optimization;
using EverythingStore.Prob;
using EverythingStore.Sell;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    public class Auction : MonoBehaviour,IPlayerInteraction
    {

		#region Field
		private AuctionItem _auctionItem;
		private AuctionManger _manager;

		[SerializeField] private ObjectPoolManger _poolManger;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private MoneySpawner _moneySpawner;


		[Title("Point")]
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _interactionPoint;

		[Title("Chair")]
		[SerializeField] private Transform _chairParent;
		[ReadOnly][SerializeField] private Chair[] _chairs;

		private int _customerReady = 0;
		private int _customerAllReady = 8;

		private bool _isCustomerReady;
		private bool _isAuctionItemReady;
		
		private List<CustomerAuction> _customerList = new();
		private Action _setAuctionItem;
		#endregion

		#region Property
		public Vector3 EnterPoint => _enterPoint.position;
		public Vector3 PickupPoint => _interactionPoint.position;
		#endregion

		#region Event
		public event Action<CustomerAuction> OnReadyCustomer;
		public event Action OnFinshAuction;
		public event Action OnPickUpAuctionItem;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_chairs = _chairParent.GetComponentsInChildren<Chair>();
			_manager = GetComponent<AuctionManger>();

			gameObject.SetActive(false);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// ������� ��� ��ǰ�� ��޸��� ���� �� ��ȣ�ۿ��� �����մϴ�.
		/// </summary>
		public void InteractionPlayer(Player player)
		{
			if(_manager.State != AuctionState.WaitAuctionItem || _auctionItem.HasItem() == true)
			{
				return;
			}

			PickupAndDrop drop = player.PickupAndDrop;
			if(drop.pickupObjectsType != PickableObjectType.SellObject)
			{
				return;
			}

			var item = drop.Drop(_auctionItem.ItemPoint, Vector3.zero).GetComponent<SellObject>();
			_auctionItem.Setup(item);
			_manager.SetUpAuction(item.Money);
		}

		/// <summary>
		/// ��ſ����� �մ��� �ε����� �����մϴ�.
		/// </summary>
		public void RegisterCustomer(CustomerAuction owner)
		{
			int index = _customerList.Count;
			bool isLeft = index % 2 == 0;

			owner.SetChair(_chairs[index], isLeft);
			_customerList.Add(owner);
		}

		/// <summary>
		/// ���� ������ ���̽��� �����մϴ�.
		/// </summary>
		public void SpawnAcutionItemCase()
		{
			_auctionItem = _poolManger.GetPoolObject(PooledObjectType.AuctionItem).GetComponent<AuctionItem>();
			_auctionItem.transform.parent = _spawnPoint;
			_auctionItem.transform.localPosition = Vector3.zero;
		}

		/// <summary>
		/// ���� �����ڰ� ���ǰ�� �������ϴ�.
		/// ������� �ݽ��ϴ�.
		/// </summary>
		public void PickupAuctionItem(PickupAndDrop _pickup)
		{
			_pickup.Pickup(_auctionItem, () => OnPickUpAuctionItem?.Invoke());
			_moneySpawner.AddMoney(_manager.BidMoney);
			_auctionItem = null;
			_manager.CloseAuction(_customerList);
			_customerList.Clear();
		}

		/// <summary>
		/// �մ��� �غ� �Ϸ�
		/// </summary>
		public void CustomerReady(CustomerAuction owner)
		{
			OnReadyCustomer?.Invoke(owner);
		}
		#endregion

		#region Private Method
		#endregion

	}
}