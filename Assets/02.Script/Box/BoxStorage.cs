using EverythingStore.BoxBox;
using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System.Collections;
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

		[Title("Box Move")]
		[SerializeField] private float _boxMoveTime;
		private IEnumerator _cBoxMove;

		[Title("Box Output")]
		[SerializeField] private OutputBox _outputBox;
		[SerializeField] private Transform _boxOutputPoint;
		#endregion

		#region Property
		public int Capacity => _capacity;
		public object Pivot => _pivot;
		public BoxStoragePointData PivotData => _pivotData;
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Start()
		{
			_outputBox.OnEmptyBox += SendToBoxOutput;
		}
		private void OnDrawGizmos()
		{
			if (_gizomo == false)
				return;

			Gizmos.color = _gizmoColor;

			GUIStyle style = new();
			style.fontSize = _fontSize;

			int count = 0;
			foreach (var item in _pivotData.Points)
			{
				Vector3 center = item + _pivot.position + Vector3.up * (_gizmoBoxSize * 0.5f);

				Handles.Label(center, count.ToString(), style);
				Gizmos.DrawWireCube(center, Vector3.one * _gizmoBoxSize);

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
			var point = GetPivotPoint(_boxQueue.Count);
			box.transform.parent = transform;
			box.transform.localPosition = point;
			box.SetInteraction(false);

			if (_outputBox.HasBox() == false)
			{
				_outputBox.SetBox(box);
			}
			else
			{
				_boxQueue.Enqueue(box);
			}
		}

		private void SendToBoxOutput()
		{
			if (_outputBox.HasBox() == true || _boxQueue.Count == 0)
			{
				return;
			}

			var box = _boxQueue.Dequeue();
			_outputBox.SetBox(box);

			OutputBox();
			return;
		}

		public void SavePointData(List<Vector3> points)
		{
			_pivotData.SavePointData(Capacity, points);
		}
		#endregion

		#region Private Method
		private Vector3 GetPivotPoint(int index)
		{
			return _pivotData.Points[index];
		}

		private void OutputBox()
		{
			if (_cBoxMove != null)
			{
				StopCoroutine(_cBoxMove);
			}
			_cBoxMove = C_BoxMove();
			StartCoroutine(_cBoxMove);
		}

		private IEnumerator C_BoxMove()
		{
			float time = 0.0f;

			while (time < _boxMoveTime)
			{
				float progress = time / _boxMoveTime;
				int index = 0;
				foreach (var item in _boxQueue)
				{
					Vector3 localPos = Vector3.Lerp(item.transform.localPosition, _pivotData.Points[index], progress);
					item.transform.localPosition = localPos;
					index++;
				}
				yield return null;
				time += Time.deltaTime;
			}
			_cBoxMove = null;
		}
		#endregion
	}
}
