using EverythingStore.Optimization;
using Sirenix.OdinInspector;
using System;
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
			ObjectPoolManger _poolManger = GameObject.FindObjectOfType<ObjectPoolManger>();

			for (int i = 0; i < _capacity; i++)
			{
				Box box = _poolManger.GetPoolObject(PooledObjectType.Box_Normal).GetComponent<Box>();
				AddBox(box);
			}
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
		/// �ڽ� ������� �ڽ��� �߰��մϴ�.
		/// </summary>
		public void AddBox(Box box)
		{
			var point = GetPivotPoint(_boxQueue.Count);
			box.transform.parent = transform;
			box.transform.localPosition = point;
			_boxQueue.Enqueue(box);
		}

		[Button("Test")]
		public Box RemoveBox()
		{
			var box = _boxQueue.Dequeue();
			box.gameObject.SetActive(false);
			BoxPointReset();
			return box;
		}



		internal void SavePointData(List<Vector3> points)
		{
			_pivotData.SavePointData(Capacity, points);
		}
		#endregion

		#region Private Method
		private Vector3 GetPivotPoint(int index)
		{
			return _pivotData.Points[index];
		}

		private void BoxPointReset()
		{
			if(_cBoxMove != null)
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
				time += Time.deltaTime;
				yield return null;
			}
		}
		#endregion
	}
}