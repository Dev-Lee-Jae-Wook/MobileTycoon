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
	public class CustomerAuction : MonoBehaviour, IAnimationEventAction
	{
		#region Field
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
		#endregion

		#region Public Method
		/// <summary>
		/// Customer�� �ʱ�ȭ�մϴ�.
		/// </summary>
		public void Init(Vector3 enterPoint)
		{
			//��ġ ����
			transform.position = enterPoint;

			//��� ����
			int money = Random.Range(100, 120);
			float  priority = Random.Range(0.2f, 1.0f);
			_participant.SetUp(money, priority);

			//���� �ӽ� ����
			_machine.StartMachine(FSMStateType.CustomerAuction_EnterAuction);
		}

		/// <summary>
		/// �մԿ��� �ʿ��� ������ �����ϰ� FSM�� �����մϴ�.
		/// </summary>
		public void Setup(Auction auction, AuctionSubmit submit,Vector3 exitPoint)
		{
			_participant = new(this, submit);
			_sensor = GetComponent<CustomerRayInteraction>();
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();
			_pooledObject = GetComponent<PooledObject>();

			//----FSM ����----
			List<IFSMState> _stateList = new();
			_stateList.Add(new MoveToPoint(_move, auction.EnterPoint, FSMStateType.CustomerAuction_EnterAuction, FSMStateType.CustomerAuction_Sitdown));
			_stateList.Add(new SitDown(this, auction, _move));
			_stateList.Add(new AuctionWait(this, auction));
			_stateList.Add(new DoAuction(this, auction));
			_stateList.Add(new AuctionResultCheck(this));
			_stateList.Add(new SuccesBid(this, auction, _move));
			_stateList.Add(new FailBid(this, auction));
			_stateList.Add(new MoveToPoint(_move, exitPoint, FSMStateType.CustomerAuction_MoveToExit, FSMStateType.ExitStore));
			_stateList.Add(new ExitStore(ExitStore));

			_isSetup = true;
			_machine.SetUpState(_stateList);
		}

		/// <summary>
		/// �մ��� ��� ���·� �����. FSM ��Ȱ��ȭ
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

		/// <summary>
		///�ɱ� �ִϸ��̼� ȣ�� 
		/// </summary>
		public void Sitdown()
		{
			_chair.Sitdown(this);
			OnAnimationSitdown?.Invoke();
		}

		public void StartAuction()
		{
			_isAuctionResult = false;
			_machine.ChangeState(FSMStateType.CustomerAuction_DoAcution);
		}

		/// <summary>
		/// �ɱ� �ִϸ��̼� ���� ȣ��
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
		#endregion
	}
}
