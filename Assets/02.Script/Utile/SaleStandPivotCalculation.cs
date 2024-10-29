using EverythingStore.InteractionObject;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

namespace EverythingStore.Util
{
	public class SaleStandPivotCalculation : MonoBehaviour
	{
		[SerializeField] private Transform _pivot;
		[SerializeField] private float _nextX;
		[SerializeField] private float _nextZ;
		[SerializeField] private int _lineNum;

		[SerializeField] private SalesStand _salesStand;
		[SerializeField] private SaleStandPivotData _pivotData;

		private List<Vector3> points;

		//인스텍터의 값이 변경이되면 호출됩니다.
		private void OnValidate()
		{
			if(_salesStand == null || _pivotData == null || _pivot == null)
			{
				return;
			}

			CalculationPivotData();
		}

		private void OnDrawGizmos()
		{
			foreach (Vector3 p in points)
			{
				Gizmos.DrawCube(p, Vector3.one * 0.1f);
			}
		}

		private void CalculationPivotData()
		{
			points.Clear();
			Vector3 point = _pivot.position;
			int capacity = _salesStand.Capacity;

			while (capacity > 0)
			{
				for (int i = 0; i < _lineNum && capacity > 0; i++)
				{
					points.Add(point);
					capacity--;
					point.x += _nextX;
				}
				point.x = _pivot.position.x;
				point.z += _nextZ;
			}

			_pivotData.SetPivotData(points);
		}

	}
}