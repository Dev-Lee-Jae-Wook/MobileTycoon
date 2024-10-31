using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.ProjectileMotion
{
	public class BezierCurveData
	{
		public Transform target { get; private set; }
		public Vector3 startPoint { get; private set; }
		public Transform endTarget { get; private set; }
		public Vector3 minPoint { get; private set; }
		public Vector3 endLocalPos { get; private set; }

		/// <summary>
		/// ������ � �������� ������ ȣ��� Callback ���� ���� Height ���� �Ķ���ͷ� �ް� �ִ�.
		/// </summary>
		public Action callback { get; private set; }

		/// <summary>
		/// �ش� Ÿ���� ����� �ð�
		/// </summary>
		public float currentTime { get; set; }

		public BezierCurveData(Transform target, Vector3 startPoint, Transform endTarget, Vector3 minPoint, Vector3 endLocalPosition, Action callback)
		{
			this.target = target;
			this.startPoint = startPoint;
			this.endTarget = endTarget;
			this.minPoint = minPoint;
			this.endLocalPos = endLocalPosition;
			this.callback = callback;
			currentTime = 0.0f;
		}
	}
}