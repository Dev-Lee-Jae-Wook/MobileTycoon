using EverythingStore.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace EverythingStore.AI
{
	[RequireComponent(typeof(NavMeshAgent))]
    public class NavmeshMove : MonoBehaviour,IAnimationEventMovement
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
			//액터가 움직이는지를 판단해서 넘겨줍니다.
			OnAnimationMovement?.Invoke(_agent.velocity.magnitude);
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 목표 위치까지 이동하고 도착하면 CallBack함수를 호출합니다.
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
		/// 도착하면 CallBack을 호출하고 해당 코루틴을 담고 있는 변수를 초기화 합니다.
		/// </summary>
		private IEnumerator C_MovePoint(Vector3 point,Action callback)
		{
			_agent.SetDestination(point);
			_agent.isStopped = false;
			yield return null;

			while(_agent.remainingDistance == Mathf.Infinity ||_agent.remainingDistance <= 0.0f)
			{
				yield return null;
			}

			//남은 거리가 멈추는 거리보다 짧은 경우 도착으로 판단
			while(_agent.remainingDistance > _agent.stoppingDistance)
			{
				yield return null;
			}
			_agent.isStopped = true;
			callback?.Invoke();

			_cMovePoint = null;
		}
		#endregion
	}
}
