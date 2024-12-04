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

namespace EverythingStore.Manager
{
	public class CustomerManager : MonoBehaviour
	{
		#region Field
		[Title("ObjectPoolManger")]
		[SerializeField] private ObjectPoolManger _poolManger;

		[Title("Customer Init Data")]
		[SerializeField] private Counter _counter;
		[SerializeField] private SalesStandManager _salesStandManager;
		[SerializeField] private Transform _exitPoint;
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _midPoint;

		[Title("AuctionCustomer Mesh")]
		[SerializeField] private CustomerMeshData _meshData;

		[Title("CustomerAuction Init Data")]
		[SerializeField] private AuctionManger _auctionManger;

		[Title("CoolTime")]
		[SerializeField] private float _coolTime;
		private float _currentCoolTime;

		[Title("Store Data")]
		[ReadOnly] public int customerCount;

		private WaitForSeconds _auctionCustomerSpawnDelay = new(0.5f);

		private List<IEnterableCustomer> _interactionObjectFullList = new();
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			foreach(var item in _salesStandManager.SalesStands)
			{
				_interactionObjectFullList.Add(item);
			}
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

		[Button("Test")]
		/// <summary>
		/// 손님을 스폰합니다.
		/// </summary>
		private bool TrySpawnCustomer()
		{
			var enterableSalesStand = _salesStandManager.EnterSalesStand();
			if(enterableSalesStand == null)
			{
				return false;
			}

			var customer = _poolManger.GetPoolObject(PooledObjectType.Customer).GetComponent<Customer>();

			if (customer.IsSetup == false) 
			{
				customer.Setup(_counter, _exitPoint.position);
			}

			customer.transform.position = transform.position;
			customer.OnExitStore += CustomerDelete;
			customer.Init(enterableSalesStand);
			customerCount++;
			return true;
		}

		private void CoolTimeUpdate()
		{
			_currentCoolTime -= Time.deltaTime;
			//쿨타임이 지난 경우
			if (_currentCoolTime <= 0.0f)
			{
				if (TrySpawnCustomer() == true)
				{
					StartWaitSpawn();
				}
			}
		}

		/// <summary>
		/// 쿨타임 초기화하고 활성화합니다.
		/// </summary>
		private void StartWaitSpawn()
		{
			_currentCoolTime = _coolTime;
		}

		private void CustomerDelete(GameObject customer)
		{
			customer.GetComponent<PooledObject>().Release();
			customerCount--;
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