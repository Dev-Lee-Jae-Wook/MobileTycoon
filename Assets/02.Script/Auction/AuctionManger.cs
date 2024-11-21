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
		/// 경매의 올라온 아이템의 가치를 설정하고 경매장을 오픈합니다.
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
		/// 입찰을 갱신합니다.
		/// </summary>
		public void UpdateBid(int money, AuctionParticipant participant)
		{
			_bidMoney = money;
			_lastBid = participant;
			_waitTime = 1.0f;
			OnUpdateBidMoney?.Invoke(_bidMoney);
		}

		/// <summary>
		/// 손님에게 연결된 경매장 이벤트를 해제합니다.
		/// </summary>
		public void ReleseCustomer(CustomerAuction customer)
		{
			OnStartAuction -= customer.StartAuction;
			OnFinshAuction -= customer.FinshAuction;
		}

		/// <summary>
		/// 경매장을 닫습니다.
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
		/// 경매를 진행 대기 시간을 넘어서도 추가적인 입찰이 없는 경우 종료됩니다.
		/// 입찰이 되면 대기 시간을 초기화 합니다.
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
		/// 경매장 상태를 변화합니다.
		/// </summary>
		private void ChangeAuctionState(AuctionState state)
		{
			_state = state;
			switch (_state)
			{
				//일정 시간 후에 다시 활성화 됩니다.
				case AuctionState.Close:
					_closeTimer.StartCoolTime(_closeTime);
					break;
				//경매 상품 케이스를 생성합니다.
				case AuctionState.WaitAuctionItem:
					_auction.SpawnAcutionItemCase();
					break;
				//손님들을 받습니다.
				case AuctionState.Open:
					_customerManager.SpawnAuctionCustomer();
					ChangeAuctionState(AuctionState.WaitCustomer);
					break;
				//손님들을 기달립니다.
				case AuctionState.WaitCustomer:
					_customerReady = 0;
					break;
				//경매를 시작합니다.
				case AuctionState.Start:
					StartAuction();
					break;
				//경매의 최종 입찰자가 결정됩니다.
				case AuctionState.Finsh:
					OnFinshAuction?.Invoke(_lastBid.Owner);
					break;
			}
		}

		/// <summary>
		/// 경매장에 손님 준비 완료
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
		/// 경매 시작
		/// </summary>
		private void StartAuction()
		{
			StartCoroutine(C_Auction());
			OnStartAuction?.Invoke();
		}

		#endregion

	}
}
