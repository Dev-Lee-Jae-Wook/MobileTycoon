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
		[SerializeField] private LockArea _lockArea;
		[SerializeField] private Transform _spawnPoint;


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
		#endregion

		#region Property
		public Vector3 EnterPoint => _enterPoint.position;
		public Vector3 PickupPoint => _interactionPoint.position;
		public AuctionManger Manger => _manager;
		public AuctionItem AuctionItem => _auctionItem;
		#endregion

		#region Event
		public event Action OnFinshAuction;
		public event Action OnSuccessBidExit;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_lockArea.OnCompelte += () => gameObject.SetActive(true);
			_chairs = _chairParent.GetComponentsInChildren<Chair>();
			_manager = GetComponent<AuctionManger>();
		}

		private void Start()
		{
			SpawnAcutionItemCase();
		}
		#endregion

		#region Public Method
		//Player가 경매 상품을 진열하면 경매장 열기
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

			var item = drop.Drop(_auctionItem.ItemPoint, Vector3.zero, OpenAuction).GetComponent<SellObject>();
			_auctionItem.Setup(item);
		}

		/// <summary>
		/// 경매에서의 손님의 인덱스를 전달합니다.
		/// </summary>
		public void RegisterCustomer(CustomerAuction owner)
		{
			int index = _customerList.Count;
			bool isLeft = index % 2 == 0;

			owner.SetChair(_chairs[index], isLeft);
			_customerList.Add(owner);
		}



		public bool IsBidder(AuctionParticipant bidder)
		{
			return _manager.LastBid == bidder;
		}
		#endregion

		#region Private Method
		private void SpawnAcutionItemCase()
		{
			_auctionItem = _poolManger.GetPoolObject(PooledObjectType.AuctionItem).GetComponent<AuctionItem>();
			_auctionItem.transform.parent = _spawnPoint;
			_auctionItem.transform.localPosition = Vector3.zero;
			_manager.WaitAuctionItem();
		}

		private void OpenAuction()
		{
			_manager.OpenAuction();
		}

		private void StartAuction()
		{
			_manager.StartAuction(_auctionItem.OrigneMoney, FinshAuction);
			foreach (var customer in _customerList)
			{
				customer.StartAuction();
			}
		}


		private void FinshAuction(AuctionParticipant participant)
		{
			OnFinshAuction?.Invoke();
			_customerList.Clear();
		}

		public void CustomerReady()
		{
			_customerReady++;
			if(_customerReady == _customerList.Count)
			{
				StartAuction();
			}
		}

		public void SucessBidExit()
		{
			OnSuccessBidExit?.Invoke();
		}

		#endregion

	}
}