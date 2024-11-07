using EverythingStore.InteractionObject;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.Util
{
	public class MoneySpawnPointCalculation : MonoBehaviour
	{
		[SerializeField] private MoneySpawner _moneySpanwer;
		[SerializeField] private Vector3 _interval;

		[Title("Debug")]
		[SerializeField] private float _debugCubeSize;
		[SerializeField] private Color _debugColor;

		private List<Vector3> points = new();

		private void OnValidate()
		{
			if(_moneySpanwer == null || _moneySpanwer.SpawnPoint == null || _moneySpanwer.SpawnPointData == null)
			{
				return;
			}

			CalculationMoneySpawnPoint();
		}

		private void OnDrawGizmos()
		{
			if(_moneySpanwer == null || _moneySpanwer.SpawnPointData.SpawnPoints.Count == 0)
			{
				return;
			}

			Gizmos.color = _debugColor;

			foreach(var point in _moneySpanwer.SpawnPointData.SpawnPoints)
			{
				Gizmos.DrawCube(point, _debugCubeSize * Vector3.one);
			}
		}

		[Button]
		private void CalculationMoneySpawnPoint()
		{
			Vector3 point = _moneySpanwer.SpawnPoint.position;
			Vector3 origin = point;
			Vector3Int spawnArea = _moneySpanwer.SpawnSize;

			List<Vector3> spawnPoints = new(_moneySpanwer.Capacity);

			for (int y = 0; y < spawnArea.y; y++)
			{
				point.z = origin.z;
				for (int z = 0; z < spawnArea.z; z++)
				{
					point.x = origin.x;
					for (int x = 0; x < spawnArea.x; x++)
					{
						spawnPoints.Add(point);
						point.x += _interval.x;
					}
					point.z += _interval.z;
				}
				point.y += _interval.y;
			}

			_moneySpanwer.SpawnPointData.SaveSpawPointData(_moneySpanwer.Capacity, spawnPoints);
		}
	}
}