using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradData", menuName = "CustomData/UpgradData")]
	public class UpgradData : ScriptableObject
	{
		[field: SerializeField] public string Name { get; private set; }
		public  List<UpgradDataStruct> UpgradList;
	}
}