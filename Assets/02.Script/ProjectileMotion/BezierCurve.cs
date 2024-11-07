using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.ProjectileMotion
{

    public class BezierCurve : MonoBehaviour
    {
		#region Field
		[SerializeField] private float _time;
		private Queue<BezierCurveData> _dataQueue = new Queue<BezierCurveData>();
		#endregion

		#region UnityCycle
		private void Update()
		{
			UpdateBezierCurve();
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 포물선 운동을 요청합니다.
		/// </summary>
		/// <param name="movementTarget">포물선 운동의 대상</param>
		/// <param name="endTarget">목표 Transfrom</param>
		/// <param name="midHeight">중간 위치의 높이 world 기준</param>
		/// <param name="endLocalPos">endTarget의 자식이 되었을 때의 LocalPosition</param>
		/// <param name="callback">포물선 움직임을 끝났을 때 호출할 메소드</param>
		public void Movement(Transform movementTarget, Transform endTarget,float midHeight, Vector3 endLocalPos, Action callback = null)
		{
			Vector3 mid = Vector3.Lerp(movementTarget.position, endTarget.position, 0.5f);
			mid.y = midHeight;

			BezierCurveData newData = new(movementTarget, movementTarget.position, endTarget, mid, endLocalPos, callback);
			_dataQueue.Enqueue(newData);
		}
		#endregion

		#region Private Method
		/// <summary>
		/// 진행중인 포물선 운동들을 업데이트를 합니다.
		/// </summary>
		private void UpdateBezierCurve()
		{
			//진행할 것이 없다면 진행하지 않는다.
			if (_dataQueue.Count == 0)
			{
				return;
			}

			int removeCount = 0;

			foreach (BezierCurveData data in _dataQueue)
			{
				if(BezierCurveMovement(data) == true)
				{
					data.callback?.Invoke();
					removeCount++;
				}
			}

			//remove
			while(removeCount > 0)
			{
				_dataQueue.Dequeue();
				removeCount--;
			}
		}

		/// <summary>
		/// 베지어 곡선을 이용해서 포물선 움직임을 진행합니다.
		/// </summary>
		/// <returns>포물선 움직임이 끝났다면  TRUE 아니라면 FALSE</returns>
		private bool BezierCurveMovement(BezierCurveData data)
		{
			//포물선 운동 종료
			if(data.currentTime > _time)
			{
				//타겟을 최종 설정
				data.target.parent = data.endTarget;
				data.target.localPosition = data.endLocalPos;
				data.target.localRotation = Quaternion.identity;
				return true;
			}

			Vector3 end = data.endTarget.position + data.endLocalPos;
			data.currentTime += Time.deltaTime;

			float t = data.currentTime / _time;
			
			Vector3 p1 = Vector3.Lerp(data.startPoint, data.minPoint, t);
			Vector3 p2 = Vector3.Lerp(data.minPoint, end, t);

			Vector3 result = Vector3.Lerp(p1, p2,t);

			data.target.position = result; 
			return false;
		}
		#endregion
	}
}
