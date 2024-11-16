using EverythingStore.AI;
using EverythingStore.AI.CustomerState;
using EverythingStore.AI.DeliveryTruckState;
using EverythingStore.Optimization;
using EverythingStore.OrderBox;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavmeshMove), typeof(FSMMachine))]
public class DeliveryTruck : MonoBehaviour
{
	#region Field
	[SerializeField] private ObjectPoolManger _poolManger;
	[Title("Point")]
	[SerializeField] private Transform _initPoint;
	[SerializeField] private Transform _arrivePoint;
	[SerializeField] private Transform _exitPoint;
	[Title("BoxStorage")]
	[SerializeField] private Transform _boxPoint;

	private FSMMachine _machine;
	private Queue<GameObject> _boxQueue = new();
	private NavmeshMove _move;
	private BoxOrderData[] _testData = { new(BoxType.Normal, 10) };
	#endregion

	#region Property
	public bool IsDelivery { get; private set; }
	public BoxOrderData[] OrderData { get; private set; }
	public ObjectPoolManger PoolManger => _poolManger;
	#endregion

	#region Event
	public event Action OnFinshDelivey;
	#endregion

	#region UnityCycle
	private void Awake()
	{
		_move = GetComponent<NavmeshMove>();
		_machine = GetComponent<FSMMachine>();

		List<IFSMState> states = new List<IFSMState>();

		states.Add(new SpawnBox(this));
		states.Add(new MoveToPoint(_move, _arrivePoint.position, FSMStateType.DeliveryTruck_MoveTo_ArrivePoint, FSMStateType.DeliveryTruck_BoxSendToStoreage));
		states.Add(new BoxSendToStoreSorage(this));
		states.Add(new MoveToPoint(_move, _exitPoint.position, FSMStateType.DeliveryTruck_MoveTo_ExitPoint, FSMStateType.DeliveryTruck_DeliveryEnd));
		states.Add(new DeliveryEnd(this));

		_machine.SetUpState(states);
	}

	#endregion

	#region Public Method
	/// <summary>
	/// 배달 프로세스를 시작합니다.
	/// </summary>
	public void StartDeliveryProcess(BoxOrderData[] orderData)
	{
		OrderData = orderData;
		_machine.StartMachine(FSMStateType.DeliveryTruck_SpawnBox);
	}

	/// <summary>
	/// 생성된 박스를 추가합니다.
	/// </summary>
	/// <param name="pooledObject"></param>
	public void AddBox(PooledObject pooledObject)
	{
		pooledObject.gameObject.SetActive(false);
		_boxQueue.Enqueue(pooledObject.gameObject);
	}

	/// <summary>
	/// 박스를 상점의 박스 저장고에게 넘겨줍니다.
	/// </summary>
	public void Delivery()
	{
		while(_boxQueue.Count > 0)
		{
			var box = _boxQueue.Dequeue();
			box.transform.parent = _boxPoint;
			box.transform.localPosition = Vector3.zero;
			box.SetActive(true);
		}
	}

	/// <summary>
	/// 초기 위치로 돌아가고 배달이 끝남을 알림니다.
	/// </summary>
	public void FinshDelivery()
	{
		transform.position = _initPoint.position;
		transform.rotation = _initPoint.rotation;
		_machine.StopMachine();
		OnFinshDelivey?.Invoke();
	}
	#endregion

	#region Private Method
	#endregion


	#region Protected Method
	#endregion

}
