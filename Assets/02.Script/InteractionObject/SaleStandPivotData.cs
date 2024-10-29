using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EverythingStore.InteractionObject
{
    [CreateAssetMenu(fileName ="NewSaleStandPivotData", menuName = "CustomData/SaleStandPivotData")]
    public class SaleStandPivotData : ScriptableObject
    {
		[field:SerializeField] public List<Vector3> PivotPoints {  get; private set; }


        /// <summary>
        /// PivotPoints�� ���� ������ �����մϴ�.
        /// </summary>
        /// <param name="PivotPoints"></param>
        public void SetPivotData(List<Vector3> pp)
        {
            PivotPoints.Clear();
            PivotPoints = pp.ToList();
        }
    }
}
