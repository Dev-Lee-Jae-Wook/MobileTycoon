using EverythingStore.Actor;
using EverythingStore.Actor.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class MoneySpawner : MonoBehaviour, IPlayerInteraction
	{
		#region Field
		[SerializeField] private Money _prefab;
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

		public void AddMoney(int money)
		{
			int spawnMoney = money;

			//»ý¼º °¡´ÉÇÑ µ· ¿ÀºêÁ§Æ®¸¦ ÃÊ°úÇÏ´Â °æ¿ì
			if (_toalMoney + money > Capacity)
			{
				spawnMoney = Capacity - _toalMoney;
			}

			SpawnMoney(spawnMoney);
			_toalMoney += money;
		}

		public void InteractionPlayer(PickupAndDrop pickupAndDrop)
		{
			if(_toalMoney == 0)
			{
				return;
			}

			Transform target = pickupAndDrop.GetPoint;
			SendToMoney(target);
		}

		private void SendToMoney(Transform target)
		{
			Debug.Log($"µ· È¹µæ {_toalMoney}");
			StartCoroutine(C_SendToMoney(target));
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
			var newMoney = Instantiate(_prefab);
			newMoney.transform.parent = SpawnPoint;
			newMoney.transform.localPosition = SpawnPointData.SpawnPoints[_moneys.Count];
			_moneys.Add(newMoney);
		}

		#endregion
	}
}
