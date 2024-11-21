using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using EverythingStore.Manger;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.AuctionSystem
{
	public enum AuctionState
	{
		Close,
		WaitAuctionItem,
		Open,
		WaitCustomer,
		Start,
		Finsh,
	}

	public class AuctionManger : MonoBehaviour
	{
		#region Field
		[SerializeField] private CustomerManager _customerManager;
		[SerializeField] private float _closeTime;
		private AuctionParticipant _lastBid;
		private int _bidMoney;
		private Auction _auction;
		private AuctionSubmit _submit;
		private AuctionParticipant[] _participants;
		private float _waitTime;
		[ReadOnly][SerializeField] private AuctionState _state;
		private CoolTime _closeTimer;
		private int _customerReady;
		private int _customerAllReady = 8;

		#endregion

		#region Property
		public int BidMoney => _bidMoney;
		public Auction Auction => _auction;
		public AuctionSubmit Submit => _submit;
		public AuctionParticipant LastBid => _lastBid;
		public AuctionState State => _state;
		#endregion

		#region Event
		private Action OnStartAuction;
		private Action<CustomerAuction> OnFinshAuction;

		public event Action<int> OnUpdateBidMoney;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_submit = new(this);
			_auction = GetComponent<Auction>();
			_closeTimer = GetComponent<CoolTime>();
			_closeTimer.OnComplete += () => ChangeAuctionState(AuctionState.WaitAuctionItem);
			_auction.OnReadyCustomer += ReadyCustomer;
		}

		private void Start()
		{
			ChangeAuctionState(AuctionState.WaitAuctionItem);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// ����� �ö�� �������� ��ġ�� �����ϰ� ������� �����մϴ�.
		/// </summary>
		/// <param name="autionItemValue"></param>
		public void SetUpAuction(int autionItemValue)
		{
			_bidMoney = autionItemValue;
			_submit.SetSubmitMinimumMoney(10);
			_waitTime = 5.0f;
			ChangeAuctionState(AuctionState.Open);
		}

		/// <summary>
		/// ������ �����մϴ�.
		/// </summary>
		public void UpdateBid(int money, AuctionParticipant participant)
		{
			_bidMoney = money;
			_lastBid = participant;
			_waitTime = 1.0f;
			OnUpdateBidMoney?.Invoke(_bidMoney);
		}

		/// <summary>
		/// �մԿ��� ����� ����� �̺�Ʈ�� �����մϴ�.
		/// </summary>
		public void ReleseCustomer(CustomerAuction customer)
		{
			OnStartAuction -= customer.StartAuction;
			OnFinshAuction -= customer.FinshAuction;
		}

		/// <summary>
		/// ������� �ݽ��ϴ�.
		/// </summary>
		public void CloseAuction(List<CustomerAuction> customerList)
		{
			foreach (var customer in customerList)
			{
				ReleseCustomer(customer);
			}

			ChangeAuctionState(AuctionState.Close);
		}

		#endregion

		#region Private Method
		/// <summary>
		/// ��Ÿ� ���� ��� �ð��� �Ѿ�� �߰����� ������ ���� ��� ����˴ϴ�.
		/// ������ �Ǹ� ��� �ð��� �ʱ�ȭ �մϴ�.
		/// </summary>
		private IEnumerator C_Auction()
		{
			_waitTime = 1.0f;
			while (_waitTime > 0.0f)
			{
				yield return null;
				_waitTime -= Time.deltaTime;
			}
			ChangeAuctionState(AuctionState.Finsh);
		}

		/// <summary>
		/// ����� ���¸� ��ȭ�մϴ�.
		/// </summary>
		private void ChangeAuctionState(AuctionState state)
		{
			_state = state;
			switch (_state)
			{
				//���� �ð� �Ŀ� �ٽ� Ȱ��ȭ �˴ϴ�.
				case AuctionState.Close:
					_closeTimer.StartCoolTime(_closeTime);
					break;
				//��� ��ǰ ���̽��� �����մϴ�.
				case AuctionState.WaitAuctionItem:
					_auction.SpawnAcutionItemCase();
					break;
				//�մԵ��� �޽��ϴ�.
				case AuctionState.Open:
					_customerManager.SpawnAuctionCustomer();
					ChangeAuctionState(AuctionState.WaitCustomer);
					break;
				//�մԵ��� ��޸��ϴ�.
				case AuctionState.WaitCustomer:
					_customerReady = 0;
					break;
				//��Ÿ� �����մϴ�.
				case AuctionState.Start:
					StartAuction();
					break;
				//����� ���� �����ڰ� �����˴ϴ�.
				case AuctionState.Finsh:
					OnFinshAuction?.Invoke(_lastBid.Owner);
					break;
			}
		}

		/// <summary>
		/// ����忡 �մ� �غ� �Ϸ�
		/// </summary>
		private void ReadyCustomer(CustomerAuction customer)
		{
			_customerReady++;
			OnStartAuction += customer.StartAuction;
			OnFinshAuction += customer.FinshAuction;
			if (_customerReady == _customerAllReady)
			{
				ChangeAuctionState(AuctionState.Start);
			}
		}

		/// <summary>
		/// ��� ����
		/// </summary>
		private void StartAuction()
		{
			StartCoroutine(C_Auction());
			OnStartAuction?.Invoke();
		}

		#endregion

	}
}
