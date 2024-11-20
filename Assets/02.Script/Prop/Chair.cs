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
		[SerializeField] private Transform _sucessPoint;
		private Transform _target;
		private CustomerAuction _customerAuction;
		#endregion

		#region Property
		public bool IsSitdown => _target == null;
		#endregion


		#region Public Method
		public void Sitdown(CustomerAuction target)
		{
			_customerAuction = target;
			_customerAuction.SetMoveActive(false);

			_target = _customerAuction.transform;
			_target.parent = _sitdownPoint;
			_target.localPosition = Vector3.zero;
			_target.rotation = Quaternion.identity;
		}

		public void Situp(bool isSucess)
		{
			_customerAuction.SetMoveActive(true);

			Vector3 point = _enterPoint.position;
			if (isSucess == true)
			{
				point = _sucessPoint.position;
			}

			_target.parent = null;
			_target.position = point;
			_target = null;
		}

		public Vector3 GetEnterPoint()
		{
			return _enterPoint.position;
		}
		#endregion
	}
}
