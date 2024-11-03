using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Actor.Customer
{
    public class WatingLine : MonoBehaviour
    {
		#region Field
		private Queue<Customer> _customerQueue = new();
		[SerializeField] private float _interval;
		private Vector3 _nextWaitingPoint;
		#endregion

		#region Property
		public int WaitingCustomerCount => _customerQueue.Count;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_nextWaitingPoint = GetPoint(0);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			for(int i = 0; i < 5; i++)
			{
				Gizmos.DrawCube(GetPoint(i), Vector3.one * 0.1f);
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
			outCustomer.OnTriggerGoToCounter();

			int index = 0;

			//한 칸 씩 앞으로 이동
			foreach(var item in _customerQueue)
			{
				item.Move.MovePoint(GetPoint(index++));
			}

			return outCustomer;
		}

		public void EnqueueCustomer(Customer customer)
		{
			customer.MovePoint(_nextWaitingPoint);
			_nextWaitingPoint = GetPoint(_customerQueue.Count);
			_customerQueue.Enqueue(customer);
		}
		#endregion

		#region Private Method
		private Vector3 GetPoint(int index)
		{
			return transform.position + Vector3.back *( _interval * index);
		}


		#endregion

		#region Protected Method
		#endregion


	}
}
