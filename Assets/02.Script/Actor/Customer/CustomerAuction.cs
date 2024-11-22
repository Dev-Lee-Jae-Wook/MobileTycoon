using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.AI.CustomerStateAuction;
using EverythingStore.Animation;
using EverythingStore.AuctionSystem;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using EverythingStore.Prob;
using EverythingStore.RayInteraction;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using ExitStore = EverythingStore.AI.ExitStore;
using Random = UnityEngine.Random;

namespace EverythingStore.Actor.Customer
{
	[RequireComponent(	typeof(CustomerRayInteraction),
											typeof(NavmeshMove),
											typeof(PickupAndDrop))]
	[RequireComponent (typeof(FSMMachine))]
	public class CustomerAuction : MonoBehaviour, IAuctionCustomerAnimationEventAction
	{
		#region Field
		[SerializeField] private SkinnedMeshRenderer _renderer;

		private FSMMachine _machine;
		private NavmeshMove _move;
		private PickupAndDrop _pickupAndDrop;
		private CustomerRayInteraction _sensor;
		private bool _isSetup = false;
		private AuctionParticipant _participant;
		private PooledObject _pooledObject;
		private Chair _chair;
		private bool _isLeft;
		private bool _isAuctionSucess;
		private bool _isAuctionResult;
		#endregion

		#region Property
		public CustomerRayInteraction Sensor => _sensor;
		public NavmeshMove Move => _move;
		public PickupAndDrop pickupAndDrop => _pickupAndDrop;
		public FSMStateType CurrentState => _machine.CurrentStateType;
		public bool IsSetup => _isSetup;
		public AuctionParticipant Participant => _participant;
		public Chair Chair => _chair;
		public bool IsShitDownPointLeft => _isLeft;
		public bool IsAuctionSucess => _isAuctionSucess;
		public bool IsAuctionResult => _isAuctionResult;
		#endregion

		#region Event
		public event Action<GameObject> OnExitStore;
		public event Action OnAnimationSitdown;
		public event Action OnAnimationSitup;
		public event Action OnAnimationSittingClap;
		public event Action OnAnimationReactionFail;
		public event Action OnAnimationRaising;
		public event Action OnAnimationReactionEnd;
		public event Action OnAnimationReactionSucess;
		#endregion

		#region Public Method
		/// <summary>
		/// Customer를 초기화합니다.
		/// </summary>
		public void Init(Vector3 enterPoint, int money, float priority, Mesh mesh)
		{
			_renderer.sharedMesh = mesh;

			//위치 설정
			transform.position = enterPoint;

			//경매 참여도 셋팅
			_participant.SetUp(money, priority);

			//상태 머신 설정
			_machine.StartMachine(FSMStateType.CustomerAuction_EnterAuction);
		}

		/// <summary>
		/// 손님에게 필요한 설정을 진행하고 FSM을 실행합니다.
		/// </summary>
		public void Setup(Auction auction, AuctionSubmit submit,Vector3 exitPoint, Vector3 midPoint)
		{
			_participant = new(this, submit);
			_sensor = GetComponent<CustomerRayInteraction>();
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();
			_pooledObject = GetComponent<PooledObject>();

			//----FSM 설정----
			List<IFSMState> _stateList = new();
			_stateList.Add(new MoveToPoint(_move, auction.EnterPoint, FSMStateType.CustomerAuction_EnterAuction, FSMStateType.CustomerAuction_Sitdown));
			_stateList.Add(new SitDown(this, auction, _move));
			_stateList.Add(new AuctionWait(this, auction));
			_stateList.Add(new DoAuction(this, auction));
			_stateList.Add(new AuctionResultCheck(this));
			_stateList.Add(new MoveToTable(this, _move, auction.PickupPoint));
			_stateList.Add(new ReactionSucessAuctionItem(this));
			_stateList.Add(new TakeAuctionItem(this, auction));
			_stateList.Add(new ReactionFail(this, auction));
			_stateList.Add(new ReactionResentful(this));
			_stateList.Add(new MoveToPoint(_move, midPoint, FSMStateType.CustomerAuction_MoveTo_MidPoint, FSMStateType.CustomerAuction_MoveToExit));
			_stateList.Add(new MoveToPoint(_move, exitPoint, FSMStateType.CustomerAuction_MoveToExit, FSMStateType.ExitStore));
			_stateList.Add(new ExitStore(ExitStore));

			_isSetup = true;
			_machine.SetUpState(_stateList);
		}

		/// <summary>
		/// 손님을 대기 상태로 만든다. FSM 비활성화
		/// </summary>
		public void Wait()
		{
			_machine.IsRunning = false;
		}
		public void MovePoint(Vector3 point)
		{
			Move.MovePoint(point);
		}

		public FSMMachine GetMachine()
		{
			return _machine;
		}

		public void StartAuction()
		{
			_isAuctionResult = false;
			_machine.ChangeState(FSMStateType.CustomerAuction_DoAcution);
		}

		/// <summary>
		///앉기 애니메이션 호출 
		/// </summary>
		public void Sitdown()
		{
			_chair.Sitdown(this);
			OnAnimationSitdown?.Invoke();
		}

		/// <summary>
		/// 앉기 애니메이션 해제 호출
		/// </summary>
		public void Situp(bool isSucess)
		{
			_chair.Situp(isSucess);
			OnAnimationSitup?.Invoke();
		}

		public void ExitStore()
		{
			_pickupAndDrop.Pop()?.GetComponent<PooledObject>().Release();
			_pooledObject.Release();
		}

		public void SetChair(Chair chair, bool isLeft)
		{
			_chair =chair;
		}

		public void FinshAuction(CustomerAuction sucessCustomer)
		{
			_isAuctionSucess = false;

			if (this == sucessCustomer)
			{
				_isAuctionSucess = true;
			}

			_isAuctionResult = true;
		}

		public void SittingCrap()
		{
			OnAnimationSittingClap?.Invoke();
		}

		public void Resentful()
		{
			OnAnimationReactionFail?.Invoke();
		}

		public void Raising()
		{
			OnAnimationRaising?.Invoke();
		}

		public void ReactionEnd()
		{
			OnAnimationReactionEnd?.Invoke();
		}

		public void ReactionSucessAuctionItem()
		{
			OnAnimationReactionSucess?.Invoke();
		}
		#endregion
	}
}
