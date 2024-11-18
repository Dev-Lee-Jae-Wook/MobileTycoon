using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Optimization
{
	public class ObjectPoolManger : MonoBehaviour
	{
		private Dictionary<PooledObjectType, ObjectPool> _poolTable = new();

		/// <summary>
		/// ���ϴ� Type�� ���߾ ������Ʈ�� ���� �� �ֽ��ϴ�.
		/// ��ȯ�ÿ��� PooledObject�� �������ּ���.
		/// </summary>
		public PooledObject GetPoolObject(PooledObjectType type)
		{
			return _poolTable[type].GetPooledObject();
		}

		private void Awake()
		{
			var pools = transform.GetComponentsInChildren<ObjectPool>();
			foreach (var pool in pools)
			{
				pool.Init();
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