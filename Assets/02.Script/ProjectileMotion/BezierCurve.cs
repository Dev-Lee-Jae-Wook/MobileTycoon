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
		/// ������ ��� ��û�մϴ�.
		/// </summary>
		/// <param name="movementTarget">������ ��� ���</param>
		/// <param name="endTarget">��ǥ Transfrom</param>
		/// <param name="midHeight">�߰� ��ġ�� ���� world ����</param>
		/// <param name="endLocalPos">endTarget�� �ڽ��� �Ǿ��� ���� LocalPosition</param>
		/// <param name="callback">������ �������� ������ �� ȣ���� �޼ҵ�</param>
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
		/// �������� ������ ����� ������Ʈ�� �մϴ�.
		/// </summary>
		private void UpdateBezierCurve()
		{
			//������ ���� ���ٸ� �������� �ʴ´�.
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
		/// ������ ��� �̿��ؼ� ������ �������� �����մϴ�.
		/// </summary>
		/// <returns>������ �������� �����ٸ�  TRUE �ƴ϶�� FALSE</returns>
		private bool BezierCurveMovement(BezierCurveData data)
		{
			//������ � ����
			if(data.currentTime > _time)
			{
				//Ÿ���� ���� ����
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
