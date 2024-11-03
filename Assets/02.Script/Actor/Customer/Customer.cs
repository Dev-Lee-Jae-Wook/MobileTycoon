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
		/// FSM ���¸� �����ϰ� StartType ���� ���¸� �����մϴ�.
		/// </summary>
		public void Setup(List<IFSMState> states, FSMStateType startType)
		{
			_machine.SetUpState(states);
			_machine.StartMachine(startType);
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

		/// <summary>
		/// Counter�� ��⿭�� ���� ��� ��⿭�� �������� �� �ش� �Լ��� ȣ���Ͻø� �˴ϴ�.
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
			//���� �ؼ� �ǸŴ��
			_stateList.Add(new MoveToPoint(this,
				_salesStationPoint.position,
				FSMStateType.Customer_MoveToSalesStation,
				FSMStateType.Customer_SaleStationWait, 
				_salesStation));
			//�ǸŴ뿡�� ���� ���
			_stateList.Add(new SaleStationWait(this));
			//���뿡 ����
			_stateList.Add(new MoveToPoint(this,
				_counterPoint.position,
				FSMStateType.Customer_MoveToCounter,
				FSMStateType.Customer_CounterDropSellObject,
				_counter.transform));
			//���뿡 ���� ��������
			_stateList.Add(new CounterDropSellObject(this));
			//���뿡�� ���
			_stateList.Add(new CounterCaculationWait(this));
			//������
			_stateList.Add(new GoToOutSide(this, _exitPoint.position));
			
			//���� ��⿭ ���
			TriggerWait counterWaitingLine = new(this, FSMStateType.Customer_MoveToCounter);
			_stateList.Add(counterWaitingLine);
			_goToCounter = counterWaitingLine;

			//���� �������� Ȯ��
			_stateList.Add(new CounterCaculationWait(this));

			//����� �̵�
			_stateList.Add(new GoToCounter(this));


			Setup(_stateList, FSMStateType.Customer_MoveToSalesStation);
		}


		#endregion
	}
}
