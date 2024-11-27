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
		/// Customer�� Customer_ChoiceSalesStand ���� ���� �����մϴ�.
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
		public void Setup(Counter counter, Vector3 exitPoint)
		{
			_move = GetComponent<NavmeshMove>();
			_pickupAndDrop = GetComponent<PickupAndDrop>();
			_machine = GetComponent<FSMMachine>();

			//----FSM ����----
			List<IFSMState> _stateList = new();

			//----�ӽ� ���߱� ����----
			_stateList.Add(new StopState(this));
			//----�ǸŴ�----
			//�ǸŴ� �Ա��� �̵�
			_stateList.Add(new EnterSalesStand(this, _move));
			//�ǸŴ� ��ȣ�ۿ� ����Ʈ�� �̵�
			_stateList.Add(new MoveToSalesStand(this, _move));
			//�ǸŴ�� ��ȣ�ۿ�
			_stateList.Add(new SalesStandPickupSellObject(this));

			//----����----
			//���� ���� ����Ʈ�� �̵�
			_stateList.Add(new MoveToPoint(Move, counter.EnterPoint, FSMStateType.Customer_Enter_Counter, FSMStateType.Customer_EnterCounter));
			//���뿡 ����
			_stateList.Add(new EnterCounter(this, FSMStateType.Customer_EnterCounter, counter));
			//����� �̵�
			_stateList.Add(new MoveToPoint(Move, counter.InteractionPoint, FSMStateType.Customer_MoveTo_Counter, FSMStateType.Customer_Counter_DropSellObject));
			//���뿡 ���� ��ǰ ��������
			_stateList.Add(new CounterDropSellObject(this, counter));
			//����� ��ǰ�� �ޱ� ���� ���
			_stateList.Add(new CounterWaitSendPackage(this));
			//���Կ��� ������
			_stateList.Add(new MoveToPoint(Move, exitPoint, FSMStateType.Customer_GoOutSide, FSMStateType.Customer_ExitStore));
			//���Կ��� ���� �Ϸ�
			_stateList.Add(new ExitStore(this));

			_machine.SetUpState(_stateList);
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
