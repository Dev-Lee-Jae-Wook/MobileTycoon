using System.Collections.Generic;
using UnityEngine;

namespace EverythingStore.Upgrad
{
	[CreateAssetMenu(fileName = "NewUpgradDataInt", menuName = "CustomData/UpgradDataInt")]
	public class UpgradData_Int : ScriptableObject
	{
		public  List<UpgradDataStruct<int>> UpgradList;
	}
}