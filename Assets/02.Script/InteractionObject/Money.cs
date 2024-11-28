using EverythingStore.Optimization;
using EverythingStore.ProjectileMotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	[RequireComponent(typeof(BezierCurve))]
    public class Money : MonoBehaviour, IPoolObject_CreateInitialization, IPoolObject_GetInitialization
    {
		#region Field
		private BezierCurve _curve;
		private PooledObject _pooledObject;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		#endregion

		#region Public Method
		public void CreateInitialization()
		{
			_curve = GetComponent<BezierCurve>();
			_pooledObject = GetComponent<PooledObject>();
		}

		public void GetPoolObjectInitialization()
		{
			transform.localRotation = Quaternion.identity;
		}

		public void Product(Transform target)
		{
			_curve.Movement(transform, target, target.position.y + 1.0f, Vector3.zero, OnProductEnd);
		}
		#endregion

		#region Private Method
		private void OnProductEnd()
		{
			_pooledObject.Release();
		}


		#endregion

		#region Protected Method
		#endregion

	}
}
