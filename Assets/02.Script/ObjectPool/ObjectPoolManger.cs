using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Optimization
{
	public class ObjectPoolManger : Singleton<ObjectPoolManger>
	{
		private Dictionary<PooledObjectType, ObjectPool> _poolTable = new();

		/// <summary>
		/// ���ϴ� Type�� ���߾ ������Ʈ�� ���� �� �ֽ��ϴ�.
		/// ��ȯ�ÿ��� PooledObject�� �������ּ���.
		/// </summary>
		public PooledObject GetPoolObject(PooledObjectType type)
		{
			if(_poolTable.ContainsKey(type) == false)
			{
				Debug.LogError($"[ObjectPool] {type} pool is null");
			}

			return _poolTable[type].GetPooledObject();
		}

		protected override void AwakeInit()
		{
			var pools = transform.GetComponentsInChildren<ObjectPool>();
			foreach (var pool in pools)
			{
				pool.Init(this);
				_poolTable.Add(pool.Type, pool);
			}
		}

#if UNITY_EDITOR
		[Button("SetupPoolName")]
		private void SetupName()
		{
			var pools = transform.GetComponentsInChildren<ObjectPool>();
			foreach (var pool in pools)
			{
				pool.gameObject.name = $"ObjectPool_{pool.Type}";
			}
		}
#endif
	}
}