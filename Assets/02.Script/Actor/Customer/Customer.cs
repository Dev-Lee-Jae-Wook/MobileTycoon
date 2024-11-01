using EverythingStore.AI;
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
		[SerializeField] private Transform _counter;
		#endregion

		#region Property
		public CustomerInteractionSensor Sensor { get; private set; }
		public CustomerMove Move { get; private set; }
		public PickupAndDrop pickupAndDrop { get; private set; }
		public FSMStateType CurrentState => _machine.CurrentStateType;
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
		#endregion

		#region Private Method
		private void Init()
		{
			List<IFSMState> _stateList = new();
			_stateList.Add(new CustomerState_MoveToPoint(this,
				_salesStationPoint.position,
				FSMStateType.Customer_MoveToSalesStation,
				FSMStateType.Customer_SaleStationWait, 
				_salesStation));
			_stateList.Add(new CustomerState_SaleStationWait(this));
			_stateList.Add(new CustomerState_MoveToPoint(this,
				_counterPoint.position,
				FSMStateType.Customer_MoveToCounter,
				FSMStateType.Customer_CounterDropSellObject,
				_counter));
			_stateList.Add(new CustomerState_CounterDropSellObject(this));
			_stateList.Add(new CustomerState_CounterCaculationWait(this));
			_stateList.Add(new CustomerState_Exit(this, _exitPoint.position));

			Setup(_stateList, FSMStateType.Customer_MoveToSalesStation);
		}


		#endregion
	}
}
