using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    [CreateAssetMenu(fileName ="NewSaleStandPivotData", menuName = "CustomData/SaleStandPivotData")]
    public class SaleStandPivotData : ScriptableObject
    {
		[field:SerializeField] public List<Vector3> PivotPoints {  get; private set; }

#if UNITY_EDITOR
        /// <summary>
        /// PivotPoints에 대한 정보를 설정합니다.
        /// </summary>
        /// <param name="PivotPoints"></param>
        public void SetPivotData(List<Vector3> pp)
        {
            PivotPoints.Clear();
            PivotPoints = pp.ToList();
			EditorUtility.SetDirty(this);
		}
#endif
    }
}
