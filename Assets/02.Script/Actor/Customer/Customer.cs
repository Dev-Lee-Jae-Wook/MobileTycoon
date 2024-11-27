using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.Animation;
using EverythingStore.InteractionObject;
using EverythingStore.Manger;
using EverythingStore.RayInteraction;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using ExitStore = EverythingStore.AI.CustomerState.ExitStore;
using Random = UnityEngine.Random;

namespace EverythingStore.Actor.Customer
{
	public class Customer : MonoBehaviour
	{
		#region Field
		private FSMMachine _machine;
		private NavmeshMove _move;
		private PickupAndDrop _pickupAndDrop;
		private bool _isSetup = false;
		private SalesStand _enterSalesStand;
		private int _buyCount; 
		#endregion

		#region Property
		public NavmeshMove Move => _move;
		public PickupAndDrop pickupAndDrop => _pickupAndDrop;
		public FSMStateType CurrentState => _machine.CurrentStateType;
		public bool IsSetup => _isSetup;
		public SalesStand EnterSalesStand => _enterSalesStand;
		public int BuyCount => _buyCount;
		#endregion

		#region Event
		public event Action<GameObject> OnExitStore;
		#endregion

		#region Public Method
		/// <summary>
		/// Customer를 Customer_ChoiceSalesStand 상태 부터 시작합니다.
		/// </summary>
		public void Init(SalesStand enterSalesStand)
		{
			_enterSalesStand = enterSalesStand;
			_machine.StartMachine(FSMStateType.Customer_Enter_SalesStand);
			_enterSalesStand.AddEnterMoveCustomer();
			//_buyCount = Random.Range(1, 3);
			_buyCount = 3;
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
		public void Setup(Counter counter, Vector3 exitPoint)
		{
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();

			//----FSM 설정----
			List<IFSMState> _stateList = new();

			//----머신 멈추기 상태----
			_stateList.Add(new StopState(this));
			//----판매대----
			//판매대 입구로 이동
			_stateList.Add(new EnterSalesStand(this, _move));
			//판매대 상호작용 포인트로 이동
			_stateList.Add(new MoveToSalesStand(this, _move));
			//판매대와 상호작용
			_stateList.Add(new SalesStandPickupSellObject(this));

			//----계산대----
			//계산대 입장 포인트로 이동
			_stateList.Add(new MoveToPoint(Move, counter.EnterPoint, FSMStateType.Customer_Enter_Counter, FSMStateType.Customer_EnterCounter));
			//계산대에 접근
			_stateList.Add(new EnterCounter(this, FSMStateType.Customer_EnterCounter, counter));
			//계산대로 이동
			_stateList.Add(new MoveToPoint(Move, counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//계산대에 구매 상품 내려놓기
			_stateList.Add(new CounterDropSellObject(this, counter));
			//포장된 상품을 받기 까지 대기
			_stateList.Add(new CounterWaitSendPackage(this));
			//가게에서 나오기
			_stateList.Add(new MoveToPoint(Move, exitPoint, FSMStateType.Customer_GoOutSide, FSMStateType.Customer_ExitStore));
			//가게에서 퇴장 완료
			_stateList.Add(new ExitStore(this));

			_machine.SetUpState(_stateList);
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
