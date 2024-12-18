using EverythingStore.InteractionObject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Customer
{
	public class WaitingLine : MonoBehaviour
	{
		private enum WaitDiraction
		{
			Forward,
			Back,
			Right,
			Left,
		}

		#region Field
		[SerializeField] private Transform _startPoint;
		[SerializeField] private int _max;
		[SerializeField] private float _interval;
		[SerializeField] private WaitDiraction _waitDiraction;
		private Queue<Customer> _customerQueue = new();
		private Vector3 _nextWaitingPoint;
		#endregion

		#region Property
		public int CustomerCount => _customerQueue.Count;
		public bool IsFull => _customerQueue.Count == _max;
		public int Max => _max;


		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_nextWaitingPoint = GetPoint(0);
		}

		private void OnDrawGizmos()
		{
			if (_startPoint == null)
				return;

			Gizmos.color = Color.yellow;
			Vector3 dir = GetDiraction();

			for (int i = 0; i < _max; i++)
			{
				Gizmos.DrawCube(GetPoint(i), Vector3.one * 0.2f);
			}
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 대기열의 가장 앞에 있는 손님을 가져옵니다.
		/// </summary>
		/// <returns></returns>
		public Customer DequeueCustomer()
		{
			var outCustomer = _customerQueue.Dequeue();
			int index = 0;

			//한 칸 씩 앞으로 이동
			foreach (var item in _customerQueue)
			{
				item.Move.MovePoint(GetPoint(index++));
			}

			_nextWaitingPoint = GetPoint(_customerQueue.Count);
			return outCustomer;
		}

		public void EnqueueCustomer(Customer customer)
		{
			customer.MovePoint(_nextWaitingPoint);
			_customerQueue.Enqueue(customer);
			_nextWaitingPoint = GetPoint(_customerQueue.Count);
		}
		#endregion

		#region Private Method
		private Vector3 GetPoint(int index)
		{
			return _startPoint.position + (GetDiraction() * (_interval * index));
		}

		private Vector3 GetDiraction()
		{
			switch (_waitDiraction)
			{
				case WaitDiraction.Forward:
					return _startPoint.forward;
				case WaitDiraction.Back:
					return -_startPoint.forward;
				case WaitDiraction.Right:
					return _startPoint.right;
				case WaitDiraction.Left:
					return -_startPoint.right;
			}
			return Vector3.zero;
		}

		#endregion

		#region Protected Method
		#endregion


	}
}
