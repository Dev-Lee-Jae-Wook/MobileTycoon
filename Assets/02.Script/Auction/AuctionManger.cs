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
		ReadyAuction,
		Start,
		Finsh,
	}

	public class AuctionManger : MonoBehaviour
	{
		#region Field
		[SerializeField] private CustomerManager _customerManager;
		[SerializeField] private float _closeTime;
		[SerializeField] private float _waitTime = 10.0f;
		private AuctionParticipant _lastBid;
		private int _bidMoney;
		private Auction _auction;
		private AuctionSubmit _submit;
		private AuctionParticipant[] _participants;
		private float _currentWaitTime;
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
		private Action<CustomerAuction> OnFinshAuction;

		public event Action<int> OnUpdateBidMoney;
		public event Action<float> OnSetupBidTimer;
		public event Action<float> OnUpdateBidTimer;
		public event Action<int> OnEndAuction;

		public event Action<float> OnUpdateCloseTime;
		public event Action<int,int> OnUpdateReadyCustomer;
		public event Action<int> OnUpdateReadyWait;

		public event Action<int> OnSetAuctionTime;

		public event  Action OnStartAuction;
		public event Action OnWaitAuctionItem;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_submit = new(this);
			_auction = GetComponent<Auction>();
			_closeTimer = GetComponent<CoolTime>();
			_closeTimer.OnUpdateTime += UpdateCloseTime;
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
			_currentWaitTime = 5.0f;
			OnSetAuctionTime?.Invoke(_bidMoney);
			ChangeAuctionState(AuctionState.Open);
		}

		/// <summary>
		/// ������ �����մϴ�.
		/// </summary>
		public void UpdateBid(int money, AuctionParticipant participant)
		{
			_bidMoney = money;
			_lastBid = participant;
			_currentWaitTime = _waitTime;
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
			_currentWaitTime = _waitTime;
			OnSetupBidTimer?.Invoke(_currentWaitTime);
			while (_currentWaitTime > 0.0f)
			{
				yield return null;
				_currentWaitTime -= Time.deltaTime;
				OnUpdateBidTimer?.Invoke(_currentWaitTime);
			}
			ChangeAuctionState(AuctionState.Finsh);
		}

		private IEnumerator C_ReadyAuction(float waitTime)
		{
			while(waitTime > 0.0f)
			{
				yield return null;
				waitTime -= Time.deltaTime;
				OnUpdateReadyWait?.Invoke((int)waitTime);
			}

			ChangeAuctionState(AuctionState.Start);
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
					OnWaitAuctionItem?.Invoke();
					break;
				//�մԵ��� �޽��ϴ�.
				case AuctionState.Open:
					_customerManager.SpawnAuctionCustomer(_bidMoney);
					ChangeAuctionState(AuctionState.WaitCustomer);
					OnUpdateReadyCustomer?.Invoke(0, 8);
					break;
				//�մԵ��� ��޸��ϴ�.
				case AuctionState.WaitCustomer:
					_customerReady = 0;
					break;
				case AuctionState.ReadyAuction:
					StartCoroutine(C_ReadyAuction(3.0f));
					break;
				//��Ÿ� �����մϴ�.
				case AuctionState.Start:
					StartAuction();
					break;
				//����� ���� �����ڰ� �����˴ϴ�.
				case AuctionState.Finsh:
					OnFinshAuction?.Invoke(_lastBid.Owner);
					OnEndAuction?.Invoke(_bidMoney);
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
			OnUpdateReadyCustomer?.Invoke(_customerReady, 8);
			if (_customerReady == _customerAllReady)
			{
				ChangeAuctionState(AuctionState.ReadyAuction);
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

		private void UpdateCloseTime(float time)
		{
			OnUpdateCloseTime?.Invoke(time);
		}

		#endregion

	}
}
