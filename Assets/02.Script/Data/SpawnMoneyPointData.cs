using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.AssetData
{
    [CreateAssetMenu(fileName = "NewSpawnMoneyPointData", menuName = "CustomData/SpawnMoneyPointData")]
    public class SpawnMoneyPointData : ScriptableObject
    {
		[field:SerializeField] public List<Vector3> SpawnPoints {  get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// SpawnPoints에 대한 정보를 설정합니다.
        /// </summary>
        /// <param name="PivotPoints"></param>
        public void SaveSpawPointData(int capacity,List<Vector3> pp)
        {
            SpawnPoints.Clear();
            SpawnPoints.Capacity = capacity;
            SpawnPoints = pp.ToList();
			EditorUtility.SetDirty(this);
		}
#endif
    }
}
