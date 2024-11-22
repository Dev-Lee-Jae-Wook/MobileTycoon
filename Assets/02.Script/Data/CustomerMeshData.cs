using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EverythingStore.AssetData
{
    [CreateAssetMenu(fileName = "NewCustomerMeshData", menuName = "CustomData/CustomerMeshData")]
    public class CustomerMeshData : ScriptableObject
    {
		[field:SerializeField] public List<Mesh> MeshList {  get; private set; }
    }
}
