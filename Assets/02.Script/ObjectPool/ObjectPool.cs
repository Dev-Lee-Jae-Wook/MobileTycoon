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
		private ObjectPoolManger _poolManger;
		#endregion

		#region Property
		public PooledObjectType Type => _prefab.Type;
		#endregion

		#region Public Method
		public void Init(ObjectPoolManger manger)
		{
			_poolManger = manger;
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
			pooledObject.transform.parent = transform;
			pooledObject.transform.localRotation = Quaternion.identity;
			pooledObject.transform.localScale = Vector3.one;
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

			//생성 시 초기화 호출
			if(popPooledObject.TryGetComponent<IPoolObject_GetInitialization>(out var init))
			{
				init.GetPoolObjectInitialization();
			}

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
			newPooledObject.name = Type.ToString();

			if(newPooledObject.TryGetComponent<IPoolObject_CreateInitialization>(out var createInit))
			{
				createInit.CreateInitialization();
			}

			if(newPooledObject.TryGetComponent<IPoolObject_SpawnObjectInitialization>(out var spawnInit))
			{
				spawnInit.SpawnObjectInitialization(_poolManger);
			}

			return newPooledObject;
		}
		#endregion

		#region Protected Method
		#endregion

	}
}
