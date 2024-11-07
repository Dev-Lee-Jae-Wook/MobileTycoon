using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Util
{
	public class SaleStandPivotCalculation : MonoBehaviour
	{
		[SerializeField] private SalesStand _salesStand;
		[SerializeField] private float _nextX;
		[SerializeField] private float _nextZ;
		[SerializeField] private int _lineNum;

		private List<Vector3> points = new();

		private void OnValidate()
		{
			if(_salesStand == null || _salesStand.Pivot == null || _salesStand.PivotData == null)
			{
				return;
			}

			if(_lineNum <= 0)
			{
				return;
			}
#if UNITY_EDITOR
			CalculationPivotData();
#endif
		}

#if UNITY_EDITOR
		private void CalculationPivotData()
		{
			if (_salesStand.PivotData != null)
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

			_salesStand.PivotData.SetPivotData(points);
		}
#endif
	}
}