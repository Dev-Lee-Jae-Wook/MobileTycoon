using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
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
	public class Customer : MonoBehaviour
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
		#endregion

		#region Public Method
		/// <summary>
		/// Customer를 초기화합니다.
		/// </summary>
		public void Init()
		{
			_machine.StartMachine(FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
		}

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

		/// <summary>
		/// 손님에게 필요한 설정을 진행하고 FSM을 실행합니다.
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="salesStand"></param>
		/// <param name="exitPoint"></param>
		public void Setup(Counter counter, SalesStand salesStand, Vector3 exitPoint)
		{
			_sensor = GetComponent<CustomerRayInteraction>();
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();

			//----FSM 설정----
			List<IFSMState> _stateList = new();

			//----머신 멈추기 상태----
			_stateList.Add(new StopState(this));
			//----판매대----
			//판매대 입장 포인트로 이동
			_stateList.Add(new MoveToPoint(Move, salesStand.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand, FSMStateType.Customer_EnterSalesStand));
			//판매대에 접근
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterSalesStand, salesStand));
			//판매대로 이동
			_stateList.Add(new MoveToPoint(Move, salesStand.GetInteractionPoint(),FSMStateType.Customer_MoveTo_SalesStation, FSMStateType.Customer_Interaction_SaleStation));
			//판매대와 상호작용
			_stateList.Add(new SaleStationInteraction(this));

			//----계산대----
			//계산대 입장 포인트로 이동
			_stateList.Add(new MoveToPoint(Move, counter.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_Counter, FSMStateType.Customer_EnterCounter));
			//계산대에 접근
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterCounter, counter));
			//계산대로 이동
			_stateList.Add(new MoveToPoint(Move, counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//계산대에 구매 상품 내려놓기
			_stateList.Add(new CounterDropSellObject(this));
			//포장된 상품을 받기 까지 대기
			_stateList.Add(new CounterWaitSendPackage(this));
			//가게에서 나오기
			_stateList.Add(new MoveToPoint(Move, exitPoint, FSMStateType.Customer_GoOutSide, FSMStateType.Customer_ExitStore));
			//가게에서 퇴장 완료
			_stateList.Add(new ExitStore(this));

			Setup(_stateList, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
			_isSetup = true;
		}

		/// <summary>
		/// FSM에 ExitStore에서 호출됩니다. 다른 곳에서 사용하지 말아주세요.
		/// </summary>
		public void Exit()
		{
			_pickupAndDrop.Clear();
			OnExitStore?.Invoke(gameObject);
		}
		#endregion
	}
}
