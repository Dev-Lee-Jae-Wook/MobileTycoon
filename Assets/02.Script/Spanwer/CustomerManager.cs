using EverythingStore.Actor.Customer;
using EverythingStore.AssetData;
using EverythingStore.AuctionSystem;
using EverythingStore.InteractionObject;
using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
		[SerializeField] private Transform _midPoint;

		[Title("AuctionCustomer Mesh")]
		[SerializeField] private CustomerMeshData _meshData;

		[Title("CustomerAuction Init Data")]
		[SerializeField] private AuctionManger _auctionManger;

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
		public void SpawnAuctionCustomer(int auctionItemValue)
		{
			StartCoroutine(C_SpawnAuctionCustomer(auctionItemValue));
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

		private IEnumerator C_SpawnAuctionCustomer(int auctionItemValue)
		{
			int customerCount = 8;
			while (customerCount > 0)
			{
				var customer = _poolManger.GetPoolObject(PooledObjectType.AuctionCustomer).GetComponent<CustomerAuction>();
				if (customer.IsSetup == false)
				{
					customer.Setup(_auctionManger.Auction, _auctionManger.Submit ,_exitPoint.position, _midPoint.position);
				}

				int money = Random.Range(auctionItemValue, auctionItemValue * 10);
				float priority = Random.Range(0.2f, 1.0f);
				int randomIndex = Random.Range(0, _meshData.MeshList.Count);
				var mesh = _meshData.MeshList[randomIndex];

				customer.Init(_enterPoint.position, money, priority, mesh);

				customer.transform.position = transform.position;
				customer.OnExitStore += CustomerDelete;
				customerCount--;
				yield return _auctionCustomerSpawnDelay;
			}
		}


		#endregion

	}
}