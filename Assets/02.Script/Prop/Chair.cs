using EverythingStore.Actor.Customer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace EverythingStore.Prob
{
    public class Chair : MonoBehaviour
    {
		#region Field
		[SerializeField] private Transform _enterPoint;
		[SerializeField] private Transform _sitdownPoint;
		private Transform _target;
		#endregion

		#region Property
		public bool IsSitdown => _target == null;
		#endregion


		#region Public Method
		public void Sitdown(CustomerAuction target)
		{
			target.Sitdown();
			_target = target.transform;
			_target.parent = _sitdownPoint;
			_target.localPosition = Vector3.zero;
			_target.rotation = Quaternion.identity;
		}

		public void GetUp()
		{
			_target.parent = null;
			_target.position = _enterPoint.position;
			_target = null;
		}

		public Vector3 GetEnterPoint()
		{
			return _enterPoint.position;
		}
		#endregion
	}
}
