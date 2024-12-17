using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AuctionSystem;
using EverythingStore.GameEvent;
using EverythingStore.InputMoney;
using EverythingStore.Optimization;
using EverythingStore.Prob;
using EverythingStore.Save;
using EverythingStore.Sell;
using EverythingStore.Timer;
using EverythingStore.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    public class Auction : MonoBehaviour,IPlayerInteraction, ISave
    {

		#region Field
		private AuctionItem _auctionItem;

		[SerializeField] private AuctionManger _manager;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private MoneySpawner _moneySpawner;
		[SerializeField] private UnlockSystem _unlockArea;


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

		private AuctionData _saveData;
		#endregion

		#region Property
		public Vector3 EnterPoint => _enterPoint.position;
		public Vector3 PickupPoint => _interactionPoint.position;

		public string SaveFileName => "AuctionData";
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
			InitSaveData();
		}

		private void Start()
		{
			if(GameEventManager.Instance.GameTarget <= GameTargetType.Auction)
			{
				_unlockArea.OnUnlock += () =>
				{
					GameEventManager.Instance.OnEvent(GameTargetType.EndTarget);
					Save();
				};
			}
			_unlockArea.Initialization(_saveData.isUnlock, _saveData.progressMoney);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 경매장이 경매 상품을 기달리고 있을 때 상호작용이 가능합니다.
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
		/// 경매에서의 손님의 인덱스를 전달합니다.
		/// </summary>
		public void RegisterCustomer(CustomerAuction owner)
		{
			int index = _customerList.Count;
			bool isLeft = index % 2 == 0;

			owner.SetChair(_chairs[index], isLeft);
			_customerList.Add(owner);
		}

		/// <summary>
		/// 옥션 아이템 케이스를 생성합니다.
		/// </summary>
		public void SpawnAcutionItemCase()
		{
			_auctionItem = ObjectPoolManger.Instance.GetPoolObject(PooledObjectType.AuctionItem).GetComponent<AuctionItem>();
			_auctionItem.transform.parent = _spawnPoint;
			_auctionItem.transform.localPosition = Vector3.zero;
		}

		/// <summary>
		/// 최종 입찰자가 경매품을 가져갑니다.
		/// 경매장을 닫습니다.
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
		/// 손님이 준비를 완료
		/// </summary>
		public void CustomerReady(CustomerAuction owner)
		{
			OnReadyCustomer?.Invoke(owner);
		}

		public void InitSaveData()
		{
			if(SaveSystem.HasSaveData(SaveFileName) == false)
			{
				_saveData = new();
				Save();
			}
			else
			{
				_saveData = SaveSystem.LoadData<AuctionData>(SaveFileName);
			}
		}

		public async void Save()
		{
			await SaveSystem.SaveData<AuctionData>(_saveData, SaveFileName);
		}
		#endregion

		#region Private Method
		#endregion

	}
}