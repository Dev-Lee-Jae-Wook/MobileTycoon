using EverythingStore.Actor.Customer;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace EverythingStore.Manger
{
	public class CustomerManager : MonoBehaviour
	{
		#region Field
		[Title("ObjectPoolManger")]
		[SerializeField] private ObjectPoolManger _poolManger;

		[Title("Customer Init Data")]
		[SerializeField] private Counter _counter;
		[SerializeField] private SalesStand _salesStand;
		[SerializeField] private Transform _exitPoint;
		[SerializeField] private Transform _enterPoint;

		[Title("CustomerAuction Init Data")]
		[SerializeField] private Auction _auction;

		[Title("CoolTime")]
		[SerializeField] private float _coolTime;
		private bool _isCoolTime;
		private float _currentCoolTime;

		[Title("Store Data")]
		[SerializeField] private int _maxCustomer;
		[ReadOnly] public int customerCount;

		private WaitForSeconds _auctionCustomerSpawnDelay = new(0.5f);
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			StartWaitSpawn();
		}

		private void Update()
		{
			CoolTimeUpdate();
		}

		#endregion

		#region Public Method
		public void SpawnAuctionCustomer()
		{
			StartCoroutine(C_SpawnAuctionCustomer());
		}

		#endregion

		#region Private Method
		/// <summary>
		/// 손님을 스폰합니다.
		/// </summary>
		private void SpawnCustomer()
		{
			var customer = _poolManger.GetPoolObject(PooledObjectType.Customer).GetComponent<Customer>();

			if (customer.IsSetup == false) 
			{
				customer.Setup(_counter, _salesStand, _exitPoint.position);
			}
			else
			{
				customer.Init();
			}

			customer.transform.position = transform.position;
			customer.OnExitStore += CustomerDelete;
			customerCount++;

			//최대 손님를 넘지 않았다면 쿨타임을 돌려라
			if (IsFullCustomer() == false)
			{
				StartWaitSpawn();
			}
		}

		private void CoolTimeUpdate()
		{
			//쿨타임이 아니면 반환
			if (_isCoolTime == false)
			{
				return;
			}

			_currentCoolTime -= Time.deltaTime;
			//쿨타임이 지난 경우
			if (_currentCoolTime <= 0.0f)
			{
				_isCoolTime = false;
				SpawnCustomer();
			}
		}

		/// <summary>
		/// 쿨타임 초기화하고 활성화합니다.
		/// </summary>
		private void StartWaitSpawn()
		{
			_isCoolTime = true;
			_currentCoolTime = _coolTime;
		}

		private void CustomerDelete(GameObject customer)
		{
			bool isFull = IsFullCustomer();

			customer.GetComponent<PooledObject>().Release();
			customerCount--;

			if(isFull == true)
			{
				StartWaitSpawn();
			}
		}

		private bool IsFullCustomer()
		{
			return customerCount >= _maxCustomer;
		}

		private IEnumerator C_SpawnAuctionCustomer()
		{
			int customerCount = 8;
			while (customerCount > 0)
			{
				var customer = _poolManger.GetPoolObject(PooledObjectType.AuctionCustomer).GetComponent<CustomerAuction>();
				if (customer.IsSetup == false)
				{
					Debug.Log(_exitPoint.position);
					customer.Setup(_auction, _exitPoint.position);
				}
				customer.Init(_enterPoint.position);

				customer.transform.position = transform.position;
				customer.OnExitStore += CustomerDelete;
				customerCount--;
				yield return _auctionCustomerSpawnDelay;
			}
		}


		#endregion

	}
}