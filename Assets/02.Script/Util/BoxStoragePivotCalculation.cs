using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Util
{
	public class BoxStoragePivotCalculation : MonoBehaviour
	{
		[SerializeField] private BoxStorage _boxStorage;
		[SerializeField] private Vector2Int _layerSize;
		/// <summary>
		/// 칸 간격
		/// </summary>
		[SerializeField] private Vector3 _boundary;

		private List<Vector3> points = new();

		private void OnValidate()
		{
			if(_boxStorage == null || _boxStorage.Pivot == null || _boxStorage.PivotData == null)
			{
				return;
			}

			if(_layerSize.sqrMagnitude <= 0)
			{
				return;
			}
#if UNITY_EDITOR
			CalculationPivotData();
#endif
		}

#if UNITY_EDITOR
		[Button("Update")]
		private void CalculationPivotData()
		{
			if (_boxStorage.PivotData != null)
			{
				points.Clear();
			}
			else
			{
				points = new List<Vector3>();
			}

			points.Capacity = _boxStorage.Capacity;

			Vector3 point = Vector3.zero;

			int moveDirX = 1;
			int moveDirZ = 1;

			while (CanAdd() == true) {
				for (int z = 0; z < _layerSize.y && CanAdd(); z++)
				{
					//point 넣고 x값을 추가 마지막에 넘어간다.
					for (int x = 0; x < _layerSize.x && CanAdd(); x++)
					{
						points.Add(point);
						point.x += _boundary.x * moveDirX;
					}
					point.z += _boundary.z * moveDirZ;

					//X의 방향을 바꾸고 넘어간 x를 다시 잡아준다.
					moveDirX *= -1;
					point.x += _boundary.x * moveDirX;
				}
				point.y += _boundary.y;

				//Z의 방향을 바꾸고 넘어간 z를 다시 잡아준다.
				moveDirZ *= -1;
				point.z += _boundary.z * moveDirZ;
			}

			_boxStorage.PivotData.SavePointData(points.Capacity, points);
		}

		private bool CanAdd()
		{
			return points.Count < points.Capacity;
		}



#endif
	}
}