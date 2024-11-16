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
		/// Pool에 있었던 오브젝트가 Pool에 들어갑니다.
		/// </summary>
		public void ReturnToPool(PooledObject pooledObject)
		{
			pooledObject.gameObject.SetActive(false);
			_pool.Push(pooledObject);
		}

		/// <summary>
		/// Pool에서 오브젝트를 꺼내옵니다.
		/// </summary>
		public PooledObject GetPooledObject()
		{
			PooledObject popPooledObject = null;
			
			//Pool 남아있는 오브젝트가 없는 경우
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
		/// 생성된 오브젝트는 해당 풀을 등록됩니다.
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
