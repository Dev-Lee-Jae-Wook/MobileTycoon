using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.AssetData
{
    [CreateAssetMenu(fileName ="NewSaleStandPivotData", menuName = "CustomData/SaleStandPivotData")]
    public class SaleStandPivotData : ScriptableObject
    {
        [field:SerializeField] public Vector3 PivotLocalPos { get; private set; }

		[field:SerializeField] public List<Vector3> PivotPoints {  get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// PivotPoints에 대한 정보를 설정합니다.
        /// </summary>
        /// <param name="PivotPoints"></param>
        public void SetPivotData(Vector3 pivotLocalPos ,List<Vector3> pp)
        {
            PivotPoints.Clear();
            PivotLocalPos = pivotLocalPos;
            PivotPoints = pp.ToList();
			EditorUtility.SetDirty(this);
		}
#endif
    }
}
