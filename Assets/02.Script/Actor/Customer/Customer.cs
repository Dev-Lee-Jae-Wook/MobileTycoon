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
		/// Customer�� �ʱ�ȭ�մϴ�.
		/// </summary>
		public void Init()
		{
			_machine.StartMachine(FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
		}

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

		/// <summary>
		/// �մԿ��� �ʿ��� ������ �����ϰ� FSM�� �����մϴ�.
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

			//----FSM ����----
			List<IFSMState> _stateList = new();

			//----�ӽ� ���߱� ����----
			_stateList.Add(new StopState(this));
			//----�ǸŴ�----
			//�ǸŴ� ���� ����Ʈ�� �̵�
			_stateList.Add(new MoveToPoint(Move, salesStand.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand, FSMStateType.Customer_EnterSalesStand));
			//�ǸŴ뿡 ����
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterSalesStand, salesStand));
			//�ǸŴ�� �̵�
			_stateList.Add(new MoveToPoint(Move, salesStand.GetInteractionPoint(),FSMStateType.Customer_MoveTo_SalesStation, FSMStateType.Customer_Interaction_SaleStation));
			//�ǸŴ�� ��ȣ�ۿ�
			_stateList.Add(new SaleStationInteraction(this));

			//----����----
			//���� ���� ����Ʈ�� �̵�
			_stateList.Add(new MoveToPoint(Move, counter.EnterPoint, FSMStateType.Customer_MoveTo_EnterPoint_Counter, FSMStateType.Customer_EnterCounter));
			//���뿡 ����
			_stateList.Add(new EnterWaitingInteraction(this, FSMStateType.Customer_EnterCounter, counter));
			//����� �̵�
			_stateList.Add(new MoveToPoint(Move, counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//���뿡 ���� ��ǰ ��������
			_stateList.Add(new CounterDropSellObject(this));
			//����� ��ǰ�� �ޱ� ���� ���
			_stateList.Add(new CounterWaitSendPackage(this));
			//���Կ��� ������
			_stateList.Add(new MoveToPoint(Move, exitPoint, FSMStateType.Customer_GoOutSide, FSMStateType.Customer_ExitStore));
			//���Կ��� ���� �Ϸ�
			_stateList.Add(new ExitStore(this));

			Setup(_stateList, FSMStateType.Customer_MoveTo_EnterPoint_SalesStand);
			_isSetup = true;
		}

		/// <summary>
		/// FSM�� ExitStore���� ȣ��˴ϴ�. �ٸ� ������ ������� �����ּ���.
		/// </summary>
		public void Exit()
		{
			_pickupAndDrop.Clear();
			OnExitStore?.Invoke(gameObject);
		}
		#endregion
	}
}
