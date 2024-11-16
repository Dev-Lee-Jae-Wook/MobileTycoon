using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Optimization
{
	public class PooledObject : MonoBehaviour
	{
		[field:SerializeField] public PooledObjectType Type {  get; private set; }
		private ObjectPool _pool;

		public void Init(ObjectPool objectPool)
		{
			_pool = objectPool;
			transform.parent = _pool.transform;
			gameObject.SetActive(false);
		}

		public void Release()
		{
			_pool.ReturnToPool(this);
		}
	}
}