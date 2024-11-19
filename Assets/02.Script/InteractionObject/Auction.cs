using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
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
		private enum State
		{
			Close,
			WaitAuctionItem,
			WaitCustomer,
			Ready,
			Start,
		}

		#region Field
		[SerializeField] private LockArea _lockArea;
		[SerializeField] private ObjectPoolManger _poolManger;
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _enterPoint;
		private AuctionItem _auctionItem;
		[SerializeField] private State _state;

		[Title("Chair")]
		[SerializeField] private Transform _chairParent;
		[ReadOnly][SerializeField] private Chair[] _chairs;

		private int _customerCount = 0;
		private int _customerReady = 0;

		private List<CustomerAuction> _customerList = new();

		private bool _isCustomerReady;
		private bool _isAuctionItemReady;

		#endregion

		#region Property
		public Vector3 EnterPoint => _enterPoint.position;
		#endregion

		#region Event
		public event Action OnSetAuctionItem;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_lockArea.OnCompelte += () => gameObject.SetActive(true);
			_chairs = _chairParent.GetComponentsInChildren<Chair>();
		}

		private void Start()
		{
			WaitAuctionItem();
		}
		#endregion

		#region Public Method
		//Player�� ��� ��ǰ�� �����ϸ� ����� �̿� �մԵ��� ���´�.
		public void InteractionPlayer(Player player)
		{
			if(_state != State.WaitAuctionItem || _auctionItem.HasItem() == true)
			{
				return;
			}

			PickupAndDrop drop = player.PickupAndDrop;
			if(drop.pickupObjectsType != PickableObjectType.SellObject)
			{
				return;
			}

			var item = drop.Drop(_auctionItem.ItemPoint, Vector3.zero, WaitCustomer).GetComponent<SellObject>();
			_auctionItem.Setup(item);
		}

		/// <summary>
		/// ��ſ����� �մ��� �ε����� �����մϴ�.
		/// </summary>
		public int RegisterCustomer(CustomerAuction owner)
		{
			int register = _customerCount++;
			_customerList.Add(owner);
			return register;
		}

		/// <summary>
		/// �մ��� �ɾƾߵǴ� ������ ���� ��ġ�� ��ȯ�Ѵ�.
		/// </summary>
		public Vector3 GetChairEnterPoint(int customerIndex)
		{
			return _chairs[customerIndex].GetEnterPoint();
		}

		/// <summary>
		/// �մ��� ������ �ڸ��� �ɽ��ϴ�.
		/// ��� �մ��� �ɰ� �Ǹ� ��Ÿ� �����մϴ�.
		/// </summary>
		public void Sitdown(CustomerAuction customer, int customerIndex)
		{
			_chairs[customerIndex].Sitdown(customer);
			_customerReady++;

			if (_customerReady == _chairs.Length)
			{
				Ready();
			}
		}

		#endregion

		#region Private Method
		private void WaitAuctionItem()
		{
			_auctionItem = _poolManger.GetPoolObject(PooledObjectType.AuctionItem).GetComponent<AuctionItem>();
			_auctionItem.transform.parent = _spawnPoint;
			_auctionItem.transform.localPosition = Vector3.zero;
			_state = State.WaitAuctionItem;
		}

		private void Ready()
		{
			_state = State.Ready;
			Debug.Log("��Ž���");
		}

		private void WaitCustomer()
		{
			_state = State.WaitCustomer;
			OnSetAuctionItem?.Invoke();
		}

		#endregion

		#region Protected Method
		#endregion
	}
}