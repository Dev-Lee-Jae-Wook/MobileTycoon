using EverythingStore.Actor;
using EverythingStore.Actor.Customer;
using EverythingStore.Actor.Player;
using EverythingStore.AI;
using EverythingStore.GameEvent;
using EverythingStore.InputMoney;
using EverythingStore.Optimization;
using EverythingStore.Sell;
using EverythingStore.Timer;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;

namespace EverythingStore.InteractionObject
{
	public class Counter : MonoBehaviour, IPlayerInteraction, ICustomerInteraction, IWaitingLine, IEnterPoint, IInteractionPoint, IEnterableCustomer, IUnlock
	{
		#region Field
		[SerializeField] private ObjectPoolManger _poolManger;
		[SerializeField] private float _spawnCoolTime = 1.0f;
		[SerializeField] private int _unlockCost;

		[Title("Point")]
		[SerializeField] private Transform _spawnPoint;
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _interactionPoint;

		private MoneySpawner _moneySpawner;
		private WaitingLine _watingLine;
		[ReadOnly][SerializeField] private int _maxCustomer;

		private SellPackage _sellpackage;
		private Customer _useCustomer;
		private int _money;
		private bool _isUsedCustomer;
		private int _enterCustomerCount = 0;
		private CoolTime _spawnPackageCoolTime;
		private InputMoneyArea _addStaff;

		private bool _isFirstInteraction = false;
		private bool _isAuto = false;
		#endregion

		#region Property
		/// <summary>
		/// 구매 상품의 총합
		/// </summary>
		public int Money => _money;

		public bool IsWaitingLineEmpty => _watingLine.CustomerCount == 0;

		public Vector3 EnterPoint => _enterPoint.position;

		public Vector3 InteractionPoint => _interactionPoint.position;

		public SellPackage SellPackage => _sellpackage;
		#endregion

		#region Event
		/// <summary>
		/// 계산과정이 모두 완료되었을 때 호출됩니다.
		/// </summary>
		public event Action OnFinshCalculation;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			SetUpComponent();
			_maxCustomer = _watingLine.Max + 1;
			_spawnPackageCoolTime.OnComplete += SpawnPackage;
			_addStaff.gameObject.SetActive(false);
		}

		void Start()
		{
			SpawnPackage();
		}

		private void Update()
		{
			if(_isAuto == true)
			{
				Work();
			}
		}

		#endregion

		#region Public Method
		//손님 카운터 앞에 선 상황
		public void InteractionCustomer(PickupAndDrop hand)
		{
			if (hand.CanDrop() == false)
			{
				return;
			}

			if(IsSpawnPackage() == false)
			{
				return;
			}
			
			var sellObject = hand.Drop(_sellpackage.PackagePoint, Vector3.zero, PushSellObject).GetComponent<SellObject>();
			_money += sellObject.Money;
		}

		public void InteractionPlayer(Player player)
		{
			Work();
		}

		public bool IsEmpty()
		{
			return _useCustomer == null;
		}
		
		public bool IsWaitingEmpty()
		{
			return _useCustomer == null && _watingLine.CustomerCount == 0;
		}

		public FSMStateType EnterInteraction(Customer customer)
		{
			_useCustomer = customer;
			_isUsedCustomer = true;
			RemoveEnterMoveCustomer();
			return FSMStateType.Customer_MoveTo_Counter;
		}

		public FSMStateType EnterWaitingLine(Customer customer)
		{
			_watingLine.EnqueueCustomer(customer);
			RemoveEnterMoveCustomer();
			return FSMStateType.Stop;
		}

		public bool IsEnterable()
		{
			int totalCustomer = _enterCustomerCount + _watingLine.CustomerCount;
			if (_isUsedCustomer == true)
			{
				totalCustomer++;
			}
			return _maxCustomer > totalCustomer;
		}

		public void AddEnterMoveCustomer()
		{
			_enterCustomerCount++;
		}

		public void RemoveEnterMoveCustomer()
		{
			_enterCustomerCount--;
		}

		public void PushSellObject(PickableObject sellObject)
		{
			_sellpackage.Push(sellObject.GetComponent<PooledObject>());
		}
		#endregion

		#region Private Method

		/// <summary>
		/// 돈을 생성합니다.
		/// </summary>
		private void CreateMoney(int money)
		{
			_moneySpawner.AddMoney(money);
			if(GameEventManager.Instance.GameTarget == GameTargetType.Tutorial_Counter)
			{
				Tutorial.Instance.SpawnMoney();
			}
		}

		/// <summary>
		/// 손님에게 나가라고 요청합니다.
		/// </summary>
		private void ExitToCustomer()
		{
			Debug.Log("손님 나가라고 요청");
			_useCustomer = null;
			_isUsedCustomer = false;
			_sellpackage = null;
			_spawnPackageCoolTime.StartCoolTime(_spawnCoolTime);

			if (_watingLine.CustomerCount > 0)
			{
			_useCustomer = _watingLine.DequeueCustomer();
				_isUsedCustomer = true;
				_useCustomer.MoveToCounter();
			}
		}

		/// <summary>
		/// 패키지를 생성합니다.
		/// </summary>
		private void SpawnPackage()
		{
			_sellpackage = _poolManger.GetPoolObject(PooledObjectType.Package).GetComponent<SellPackage>();
			_sellpackage.transform.parent = _spawnPoint;
			_sellpackage.transform.localPosition = Vector3.zero;

		}

		private void SetUpComponent()
		{
			_moneySpawner = transform.GetComponentInChildren<MoneySpawner>();
			_watingLine = transform.GetComponentInChildren<WaitingLine>();
			_addStaff = transform.GetComponentInChildren<InputMoneyArea>();
			_spawnPackageCoolTime = gameObject.AddComponent<CoolTime>();
		}

		private void AutoOn()
		{
			_isAuto = true;
		}

		private void Work()
		{
			if (_sellpackage == null || _useCustomer == null)
			{
				return;
			}

			//손님이 픽업한 오브젝트가 있는 경우
			if (_useCustomer.pickupAndDrop.HasPickupObject() == true)
			{
				return;
			}

			//포장이 되어있지 않았다면 포장을 한다.
			if (_sellpackage.IsPackage == false)
			{
				_sellpackage.Package(SendToCustomer);
				if(_isFirstInteraction == false)
				{
					_isFirstInteraction = true;
					OpenUpgrad();
				}
			}
		}

		private void OpenUpgrad()
		{
			_addStaff.gameObject.SetActive(true);
		}

		private void SendToCustomer()
		{
			_useCustomer.pickupAndDrop.Pickup(_sellpackage);
			CreateMoney(_money);
			_money = 0;
			ExitToCustomer();
		}

		private bool IsSpawnPackage()
		{
			return _sellpackage != null;
		}

		public void Unlock()
		{
			AutoOn();
		}
		#endregion
	}
}

