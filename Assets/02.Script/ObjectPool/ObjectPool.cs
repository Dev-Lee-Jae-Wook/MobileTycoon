using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Optimization
{
    public class ObjectPool : MonoBehaviour
    {
		#region Field
		[SerializeField] private int _initSpawn;
		[SerializeField] private PooledObject _prefab;

		private Stack<PooledObject> _pool = new();
		#endregion

		#region Property
		public PooledObjectType Type => _prefab.Type;
		#endregion

		#region Public Method
		public void Init()
		{
			gameObject.name = $"ObjectPool_{_prefab.Type}";
			for (int i = 0; i < _initSpawn; i++)
			{
				_pool.Push(CreatePooledObject());
			}
		}

		/// <summary>
		/// Pool�� �־��� ������Ʈ�� Pool�� ���ϴ�.
		/// </summary>
		public void ReturnToPool(PooledObject pooledObject)
		{
			pooledObject.gameObject.SetActive(false);
			_pool.Push(pooledObject);
		}

		/// <summary>
		/// Pool���� ������Ʈ�� �����ɴϴ�.
		/// </summary>
		public PooledObject GetPooledObject()
		{
			PooledObject popPooledObject = null;
			
			//Pool �����ִ� ������Ʈ�� ���� ���
			if (_pool.TryPop(out popPooledObject) == false)
			{
				popPooledObject = CreatePooledObject();
			}

			popPooledObject.transform.parent = null;
			popPooledObject.gameObject.SetActive(true);

			return popPooledObject;
		}

		#endregion

		#region Private Method
		/// <summary>
		/// ������ ������Ʈ�� �ش� Ǯ�� ��ϵ˴ϴ�.
		/// </summary>
		/// <returns></returns>
		private PooledObject CreatePooledObject()
		{
			var newPooledObject = Instantiate(_prefab);
			newPooledObject.Init(this);
			return newPooledObject;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
