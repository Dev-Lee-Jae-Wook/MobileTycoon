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
		[SerializeField] private SalesStand _salesStand;
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
		public FSMMachine GetMachine()
		{
			return _machine;
		}

		/// <summary>
		/// 상태를 판매대 이동으로 변경합니다.
		/// </summary>
		public void MoveToSaleStation()
		{
			_machine.ChangeState(FSMStateType.Customer_MoveTo_SalesStation);
		}

		/// <summary>
		/// 상태를 계산대 이동으로 변경합니다.
		/// </summary>
		public void MoveToCounter()
		{
			_machine.ChangeState(FSMStateType.Customer_MoveTo_Counter);
		}
		#endregion

		#region Private Method
		private void Init()
		{
			List<IFSMState> _stateList = new();

			//----머신 멈추기 상태----
			_stateList.Add(new StopState(this));
			//----판매대----
			//판매대 입장 포인트로 이동
			_stateList.Add(new MoveToPoint(this, _salesStand.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand, FSMStateType.Customer_EnterSalesStand));
			//판매대에 접근
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterSalesStand, _salesStand));
			//판매대로 이동
			_stateList.Add(new MoveToPoint(this, _salesStand.GetInteractionPoint(),FSMStateType.Customer_MoveTo_SalesStation, FSMStateType.Customer_Interaction_SaleStation));
			//판매대와 상호작용
			_stateList.Add(new SaleStationInteraction(this));
			
			//----계산대----
			//계산대 입장 포인트로 이동
			_stateList.Add(new MoveToPoint(this, _counter.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_Counter, FSMStateType.Customer_EnterCounter));
			//계산대에 접근
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterCounter, _counter));
			//계산대로 이동
			_stateList.Add(new MoveToPoint(this, _counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//계산대에 구매 상품 내려놓기
			_stateList.Add(new CounterDropSellObject(this));
			//포장된 상품을 받기 까지 대기
			_stateList.Add(new CounterWaitSendPackage(this));
			//나가기
			_stateList.Add(new MoveToPoint(this, _exitPoint.position, FSMStateType.Customer_GoOutSide, FSMStateType.Stop));


			Setup(_stateList, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
		}







		#endregion
	}
}
