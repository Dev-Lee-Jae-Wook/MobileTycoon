using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.InteractionObject;
using EverythingStore.Sensor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Customer
{
	[RequireComponent(	typeof(CustomerInteractionSensor),
											typeof(CustomerMove),
											typeof(PickupAndDrop))]
	[RequireComponent (typeof(FSMMachine))]
	public class Customer : MonoBehaviour
	{
		#region Field
		private FSMMachine _machine;

		[Title("MovePoint")]
		[SerializeField] private Transform _salesStationPoint;
		[SerializeField] private Transform _counterPoint;
		[SerializeField] private Transform _exitPoint;
		[Title("LookAtTarget")]
		[SerializeField] private Transform _salesStation;
		[SerializeField] private Counter _counter;

		private ITrigger _goToCounter;
		#endregion

		#region Property
		public CustomerInteractionSensor Sensor { get; private set; }
		public CustomerMove Move { get; private set; }
		public PickupAndDrop pickupAndDrop { get; private set; }
		public FSMStateType CurrentState => _machine.CurrentStateType;

		public Counter Counter => _counter;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{	
			Sensor =  GetComponent<CustomerInteractionSensor>();
			Move =	GetComponent<CustomerMove>();
			pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();
		}

		private void Start()
		{
			Init();
		}


		#endregion

		#region Public Method
		/// <summary>
		/// FSM 상태를 설정하고 StartType 부터 상태를 시작합니다.
		/// </summary>
		public void Setup(List<IFSMState> states, FSMStateType startType)
		{
			_machine.SetUpState(states);
			_machine.StartMachine(startType);
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

		/// <summary>
		/// Counter에 대기열에 있을 경우 대기열에 빠져나올 떄 해당 함수를 호출하시면 됩니다.
		/// </summary>
		public void OnTriggerGoToCounter()
		{
			_goToCounter.OnTrigger();
		}

		#endregion

		#region Private Method
		private void Init()
		{
			List<IFSMState> _stateList = new();
			//입장 해서 판매대로
			_stateList.Add(new MoveToPoint(this,
				_salesStationPoint.position,
				FSMStateType.Customer_MoveToSalesStation,
				FSMStateType.Customer_SaleStationWait, 
				_salesStation));
			//판매대에서 물건 대기
			_stateList.Add(new SaleStationWait(this));
			//계산대에 입장
			_stateList.Add(new MoveToPoint(this,
				_counterPoint.position,
				FSMStateType.Customer_MoveToCounter,
				FSMStateType.Customer_CounterDropSellObject,
				_counter.transform));
			//계산대에 물건 내려놓기
			_stateList.Add(new CounterDropSellObject(this));
			//계산대에서 대기
			_stateList.Add(new CounterCaculationWait(this));
			//나가기
			_stateList.Add(new GoToOutSide(this, _exitPoint.position));
			
			//계산대 대기열 대기
			TriggerWait counterWaitingLine = new(this, FSMStateType.Customer_MoveToCounter);
			_stateList.Add(counterWaitingLine);
			_goToCounter = counterWaitingLine;

			//계산대 가기전에 확인
			_stateList.Add(new CounterCaculationWait(this));

			//계산대로 이동
			_stateList.Add(new GoToCounter(this));


			Setup(_stateList, FSMStateType.Customer_MoveToSalesStation);
		}


		#endregion
	}
}
