using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.AI.CustomerStateAuction;
using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using EverythingStore.RayInteraction;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

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
		#endregion

		#region Property
		public CustomerRayInteraction Sensor => _sensor;
		public NavmeshMove Move => _move;
		public PickupAndDrop pickupAndDrop => _pickupAndDrop;
		public FSMStateType CurrentState => _machine.CurrentStateType;
		public bool IsSetup => _isSetup;
		#endregion

		#region Event
		public event Action<GameObject> OnExitStore;
		public event Action OnAnimationSitdown;
		#endregion

		void Start()
		{
			Auction auction = FindAnyObjectByType<Auction>();

			Setup( auction, Vector3.zero);
			Init();
		}


		#region Public Method
		/// <summary>
		/// Customer를 초기화합니다.
		/// </summary>
		public void Init()
		{
			_machine.StartMachine(FSMStateType.CustomerAuction_EnterAuction);
		}

		/// <summary>
		/// 손님에게 필요한 설정을 진행하고 FSM을 실행합니다.
		/// </summary>
		public void Setup(Auction auction, Vector3 exitPoint)
		{
			_sensor = GetComponent<CustomerRayInteraction>();
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();

			//----FSM 설정----
			List<IFSMState> _stateList = new();
			_stateList.Add(new MoveToPoint(_move, auction.EnterPoint, FSMStateType.CustomerAuction_EnterAuction, FSMStateType.CustomerAuction_Sitdown));
			_stateList.Add(new SitDown(this, auction, _move));
			_stateList.Add(new AuctionWait(this, auction));

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

		/// <summary>
		///앉기 애니메이션 호출 
		/// </summary>
		public void Sitdown()
		{
			OnAnimationSitdown?.Invoke();
		}
		#endregion
	}
}
