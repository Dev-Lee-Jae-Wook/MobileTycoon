using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
	public class BoxStorage : MonoBehaviour
	{
		#region Field		
		[Title("Gizmos")]
		[SerializeField] private bool _gizomo;
		[SerializeField] private float _gizmoBoxSize;
		[SerializeField] private Color _gizmoColor;
		[SerializeField] private int _fontSize;

		[Title("Point Data")]
		[SerializeField] private Transform _pivot;
		[SerializeField] private int _capacity;
		[SerializeField] private BoxStoragePointData _pivotData;
		private Queue<Box> _boxQueue = new();
		#endregion

		#region Property
		public int Capacity => _capacity;
		public object Pivot => _pivot;
		public BoxStoragePointData PivotData => _pivotData;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void OnDrawGizmos()
		{
			if (_gizomo == false)
				return;

			Gizmos.color = _gizmoColor;

			GUIStyle style = new();
			style.fontSize = _fontSize;

			int count = 0;
			foreach(var item in _pivotData.Points)
			{
				Vector3 center = item + _pivot.position + Vector3.up * (_gizmoBoxSize * 0.5f);
				Vector3 handlePoint = center + Vector3.right * _gizmoBoxSize;

				Handles.Label(handlePoint, count.ToString(), style);
				Gizmos.DrawCube(center, Vector3.one * _gizmoBoxSize);

				count++;
			}
		}
		#endregion

		#region Public Method
		/// <summary>
		/// 박스 저장고에 박스를 추가합니다.
		/// </summary>
		public void AddBox(Box box)
		{
			box.transform.parent = transform;
			_boxQueue.Enqueue(box);
		}

		internal void SavePointData(List<Vector3> points)
		{
			_pivotData.SavePointData(Capacity, points);
		}
		#endregion

		#region Private Method
		#endregion
	}
}
