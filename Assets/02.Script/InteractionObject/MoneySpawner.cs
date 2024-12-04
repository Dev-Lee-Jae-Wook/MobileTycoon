using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using EverythingStore.AssetData;
using EverythingStore.GameEvent;
using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class MoneySpawner : MonoBehaviour, IPlayerInteraction
	{
		#region Field
		[SerializeField] private ObjectPoolManger _poolManger;
		public int _toalMoney;
		private List<Money> _moneys = new();
		#endregion

		#region Property
		[field:SerializeField] public Vector3Int SpawnSize { get; set; }
		[field:SerializeField] public Transform SpawnPoint { get; private set; }
		[field: SerializeField] public SpawnMoneyPointData SpawnPointData { get; private set; }
		public int Capacity => SpawnSize.x * SpawnSize.y * SpawnSize.z;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_moneys.Capacity = Capacity;
		}
		#endregion

		#region Public Method
		[Button("AA")]		
		private void Test()
		{
			AddMoney(100);
		}


		public void AddMoney(int money)
		{
			int spawnMoney = money;

			//생성 가능한 돈 오브젝트를 초과하는 경우
			if (_toalMoney + money > Capacity)
			{
				spawnMoney = Capacity - _toalMoney;
			}

			SpawnMoney(spawnMoney);
			_toalMoney += money;
		}

		public void InteractionPlayer(Player player)
		{
			if(_toalMoney == 0)
			{
				return;
			}

			SendToMoney(player);
		}

		private void SendToMoney(Player player)
		{

			player.Wallet.AddMoney(_toalMoney);
			StartCoroutine(C_SendToMoney(player.GetItemPoint));
			_toalMoney = 0;
		}
		#endregion

		#region Private Method

		private IEnumerator C_SendToMoney(Transform target)
		{
			for(int i = _moneys.Count -1; i >= 0; i--)
			{
				_moneys[i].Product(target);
				yield return new WaitForSeconds(0.01f);
			}

			_moneys.Clear();
			if(Tutorial.Instance.isGetMoney == false)
			{
				GameEventManager.Instance.OnEvent(GameEventType.Totorial_BoxOrder);
				Tutorial.Instance.isGetMoney = true;
			}
		}

		private void SpawnMoney(int spawnMoney)
		{
			while(spawnMoney > 0)
			{
				InstanlateMoney();
				spawnMoney--;
			}
		}

		private void InstanlateMoney()
		{
			var newMoney = _poolManger.GetPoolObject(PooledObjectType.Money).GetComponent<Money>();
			newMoney.transform.parent = SpawnPoint;
			newMoney.transform.localPosition = SpawnPointData.SpawnPoints[_moneys.Count];
			_moneys.Add(newMoney);
		}

		#endregion
	}
}
