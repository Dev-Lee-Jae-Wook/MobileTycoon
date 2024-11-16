using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    [CreateAssetMenu(fileName = "NewBoxStoragePointData", menuName = "CustomData/BoxStoragePointData")]
    public class BoxStoragePointData : ScriptableObject
    {
		[field:SerializeField] public List<Vector3> Points {  get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// SpawnPoints�� ���� ������ �����մϴ�.
        /// </summary>
        /// <param name="PivotPoints"></param>
        public void SavePointData(int capacity,List<Vector3> pp)
        {
            Points.Clear();
            Points.Capacity = capacity;
            Points = pp.ToList();
			EditorUtility.SetDirty(this);
		}
#endif
    }
}
