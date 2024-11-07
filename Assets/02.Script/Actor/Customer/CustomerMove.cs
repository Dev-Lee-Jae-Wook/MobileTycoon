using EverythingStore.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EverythingStore.Actor.Customer
{
	[RequireComponent(typeof(NavMeshAgent))]
    public class CustomerMove : MonoBehaviour,IAnimationEventMovement
    {
		#region Field
		[SerializeField] private float _speed;
		private NavMeshAgent _agent;
		private IEnumerator _cMovePoint;
		#endregion

		#region Property
		#endregion

		#region Event
		public event Action<float> OnAnimationMovement;
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_agent.speed = _speed;
		}

		private void LateUpdate()
		{
			//���Ͱ� �����̴����� �Ǵ��ؼ� �Ѱ��ݴϴ�.
			OnAnimationMovement?.Invoke(_agent.velocity.magnitude);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// ��ǥ ��ġ���� �̵��ϰ� �����ϸ� CallBack�Լ��� ȣ���մϴ�.
		/// </summary>
		public void MovePoint(Vector3 point,Action callBack = null)
		{
			if(_cMovePoint != null)
			{
				StopCoroutine(_cMovePoint);
			}
			_cMovePoint = C_MovePoint(point,  callBack);
			StartCoroutine(_cMovePoint);
		}
		#endregion

		#region Private Method
		/// <summary>
		/// �����ϸ� CallBack�� ȣ���ϰ� �ش� �ڷ�ƾ�� ��� �ִ� ������ �ʱ�ȭ �մϴ�.
		/// </summary>
		private IEnumerator C_MovePoint(Vector3 point,Action callback)
		{
			_agent.SetDestination(point);
			yield return null;
			//���� �Ÿ��� ���ߴ� �Ÿ����� ª�� ��� �������� �Ǵ�
			while(_agent.remainingDistance > _agent.stoppingDistance)
			{
				yield return null;
			}
			callback?.Invoke();

			_cMovePoint = null;
		}
		#endregion
	}
}