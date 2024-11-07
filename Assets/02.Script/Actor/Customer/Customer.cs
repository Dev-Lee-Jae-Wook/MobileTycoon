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
		public FSMMachine GetMachine()
		{
			return _machine;
		}

		/// <summary>
		/// ���¸� �ǸŴ� �̵����� �����մϴ�.
		/// </summary>
		public void MoveToSaleStation()
		{
			_machine.ChangeState(FSMStateType.Customer_MoveTo_SalesStation);
		}

		/// <summary>
		/// ���¸� ���� �̵����� �����մϴ�.
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

			//----�ӽ� ���߱� ����----
			_stateList.Add(new StopState(this));
			//----�ǸŴ�----
			//�ǸŴ� ���� ����Ʈ�� �̵�
			_stateList.Add(new MoveToPoint(this, _salesStand.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand, FSMStateType.Customer_EnterSalesStand));
			//�ǸŴ뿡 ����
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterSalesStand, _salesStand));
			//�ǸŴ�� �̵�
			_stateList.Add(new MoveToPoint(this, _salesStand.GetInteractionPoint(),FSMStateType.Customer_MoveTo_SalesStation, FSMStateType.Customer_Interaction_SaleStation));
			//�ǸŴ�� ��ȣ�ۿ�
			_stateList.Add(new SaleStationInteraction(this));
			
			//----����----
			//���� ���� ����Ʈ�� �̵�
			_stateList.Add(new MoveToPoint(this, _counter.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_Counter, FSMStateType.Customer_EnterCounter));
			//���뿡 ����
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterCounter, _counter));
			//����� �̵�
			_stateList.Add(new MoveToPoint(this, _counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//���뿡 ���� ��ǰ ��������
			_stateList.Add(new CounterDropSellObject(this));
			//����� ��ǰ�� �ޱ� ���� ���
			_stateList.Add(new CounterWaitSendPackage(this));
			//������
			_stateList.Add(new MoveToPoint(this, _exitPoint.position, FSMStateType.Customer_GoOutSide, FSMStateType.Stop));


			Setup(_stateList, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
		}







		#endregion
	}
}
