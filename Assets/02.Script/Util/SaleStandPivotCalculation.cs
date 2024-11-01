using EverythingStore.InteractionObject;
using System.Collections.Generic;
using UnityEngine;

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

		private List<Vector3> points = new();

		//인스텍터의 값이 변경이되면 호출됩니다.
		private void OnValidate()
		{
			if (_salesStand == null || _pivotData == null || _pivot == null)
			{
				return;
			}

			CalculationPivotData();
		}

		private void OnDrawGizmos()
		{
			if (points == null)
			{
				return;
			}

			foreach (Vector3 p in points)
			{
				Vector3 worldPos = _pivot.position + p;
				Gizmos.DrawCube(worldPos, Vector3.one * 0.1f);
			}
		}

		private void CalculationPivotData()
		{
			if (_pivotData != null)
			{
				points.Clear();
			}
			else
			{
				points = new List<Vector3>();
			}

			Vector3 point = Vector3.zero;
			int capacity = _salesStand.Capacity;

			while (capacity > 0)
			{
				for (int i = 0; i < _lineNum && capacity > 0; i++)
				{
					points.Add(point);
					capacity--;
					point.x += _nextX;
				}
				point.x = 0.0f;
				point.z += _nextZ;
			}

			_pivotData.SetPivotData(points);
		}

	}
}